﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro PageImport2.xaml
    /// </summary>
    public partial class PageImport2 : Page
    {
        public PageImport2()
        {
            InitializeComponent();

            btnImportObjektOdstranit.IsEnabled = false;
        }

        // POMOCNÉ PROMĚNNÉ:

        // příznak pro přidávání objektu
        private bool pridavaniObjektu = false;
        // příznak pro přidávání objektu
        private bool pridavaniVazby = false;

        // aktuálně vybraný objekt
        private Objekt vybranyObjekt = null;
        // aktuálně vybraná vazba
        private Vazba vybranaVazba = null;

        // bod, kde začíná vazba
        private Point? startVazbyPoint = null;

        // Proměnné pro detekci pohybu
        private bool presouvani = false;
        private Point posledniPozice;

        // aktuální ID
        private int aktualniID = 1;

        // pomocná vazba
        Line line0;


        // SEZNAMY a LISTY OBJEKTŮ:

        // seznam uložených Objektů
        private List<Objekt> vsechnyObjekty = new List<Objekt>();
        // seznam uložených Vazeb
        private List<Vazba> vsechnyVazby = new List<Vazba>();



        // POMOCNÉ METODY a FUNKCE:

        // Odznačí všechny Objekty
        private void OdznacObjekty()
        {
            foreach (var objekt in vsechnyObjekty)
            {
                objekt.ResetAppearance(); // Restartuje vlastnosti všech objektů
            }
        }
        // Odznačí všechny Vazby
        private void OdznacVazby()
        {
            foreach (var vazba in vsechnyVazby)
            {
                vazba.ResetAppearance(); // Restartuje vlastnosti všech vazeb
            }
        }
        // HIT TEST - najdi, na který prvek v Canvas se kliklo
        private Objekt MyHitTest(object sender, MouseButtonEventArgs e, Type shape)
        {
            Objekt endObjekt = null;

            // Vytvoření seznamu pro uložení všech prvků, které byly detekovány
            List<UIElement> hitObjects = new List<UIElement>();

            // Provedení hittestingu
            VisualTreeHelper.HitTest(
                sender as Canvas,
                null,
                new HitTestResultCallback(result =>
                {
                    if (result.VisualHit is UIElement element && cnvsObjekty.Children.Contains(element))
                    {
                        hitObjects.Add(element);
                    }
                    return HitTestResultBehavior.Continue;
                }),
                new PointHitTestParameters(e.GetPosition((UIElement)sender))
            );

            // Procházení seznamu a zpracování výsledků
            foreach (var hit in hitObjects)
            {
                if (hit.GetType() == shape)
                {
                    endObjekt = vsechnyObjekty.FirstOrDefault(obj => obj.Shape == hit);
                    break;
                }
            }

            return endObjekt;
        }

        // TLAČÍTKA:

        // PO KLIKU NA TLAČÍTKO PRO VYTVOŘENÍ NOVÉHO OBJEKTU:
        private void btnImportNovyObjekt_Click(object sender, RoutedEventArgs e)
        {
            // Nastavte příznak pro přidávání objektu
            pridavaniObjektu = true;
            pridavaniVazby = false;

            // tlačítko bude svítit modře
            btnImportNovyObjekt.Background = Brushes.LightSkyBlue;
            // Resetuj barvu tlačítka
            btnImportNovaVazba.Background = Brushes.LightGray;
        }

        // PO KLIKU NA TLAČÍTKO PRO VYTVOŘENÍ NOVÉ VAZBY:
        private void btnImportNovaVazba_Click(object sender, RoutedEventArgs e)
        {
            OdznacObjekty();
            OdznacVazby();

            // Nastavte příznak pro přidávání vazby
            pridavaniVazby = true;
            pridavaniObjektu = false;

            // ukončIT režim přesouvání:
            presouvani = false; // Deaktivujte režim přesouvání

            // tlačítko bude svítit modře
            btnImportNovaVazba.Background = Brushes.LightSkyBlue;
            // Resetuj barvu tlačítka
            btnImportNovyObjekt.Background = Brushes.LightGray;
        }

        // PO KLIKU NA TLAČÍTKO ODSTRANIT:
        private void btnImportObjektOdstranit_Click(object sender, RoutedEventArgs e)
        {
            // Pokud je nějaký objekt vybrán, odeberte ho z Canvasu
            if (vybranyObjekt != null)
            {
                cnvsObjekty.Children.Remove(vybranyObjekt.Shape); // Odebrání Obejktu
                cnvsObjekty.Children.Remove(vybranyObjekt.Popisek); // Odebrání popisku

                vybranyObjekt = null; // resetujte označený objekt
            }

            // Pokud je nějaká vazba vybrána, odeberte ji z Canvasu
            if (vybranaVazba != null)
            {
                cnvsObjekty.Children.Remove(vybranaVazba.Line); // Odebrání Vazby

                vybranaVazba = null; // resetujte označenou vazbu
            }

            // Resetujte barvy tlačítek (pokud máte nějaké jiné vizuální indikátory pro tlačítka)
            btnImportNovyObjekt.Background = Brushes.LightGray;
            btnImportNovaVazba.Background = Brushes.LightGray;
            btnImportObjektOdstranit.IsEnabled = false;
        }

        // METODY A FUNKCE:

        // PO KLIKNUTÍ LEVÝM TLAČÍTKEM DO PLÁTNA CANVAS:
        private void cnvsObjekty_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIElement clickedElement = e.OriginalSource as UIElement;

            Point clickPoint = e.GetPosition(cnvsObjekty);

            // Pokud se PŘIDÁVÁ NOVÝ OBJEKT:
            if (pridavaniObjektu)
            {
                // Získání pozice kliknutí myší
                Point position = clickPoint;//e.GetPosition(cnvsObjekty);

                // Přidejte bílý kruh do Canvas na pozici kliknutí
                Ellipse newCircle = new Ellipse();
                newCircle.Fill = Brushes.White;
                newCircle.Stroke = Brushes.Black;
                newCircle.Width = 50;
                newCircle.Height = 50;

                // Nastavte pozici kruhu na pozici kliknutí
                Canvas.SetLeft(newCircle, position.X - newCircle.Width / 2);
                Canvas.SetTop(newCircle, position.Y - newCircle.Height / 2);

                // Vytvořte popisek a nastavte jeho hodnoty
                TextBlock popisek = new TextBlock();
                popisek.Text = "ID=" + aktualniID; // Používáme hodnotu aktualniID, kterou jsme nastavili dříve
                popisek.Foreground = Brushes.Black;
                Canvas.SetLeft(popisek, position.X - newCircle.Width / 2); // (toto by bylo hned vlevo pod elipsou)
                //Canvas.SetLeft(popisek, position.X - popisek.ActualWidth / 2); // popisek začne!! vprostřed pod elipsou
                Canvas.SetLeft(popisek, position.X - newCircle.Width / 2 + 10); // (toto by bylo hned vlevo pod elipsou) + 10 odsazení
                Canvas.SetTop(popisek, position.Y + newCircle.Height / 2 + 5); // +5 je pro malý odsazení od elipsy

                // Přidejte kruh do Canvas
                cnvsObjekty.Children.Add(newCircle);
                cnvsObjekty.Children.Add(popisek);

                // Uložte informace o objektu
                // NEW:
                Objekt novyObjekt = new Objekt(aktualniID++, newCircle);
                novyObjekt.Popisek = popisek;
                // OLD:
                //Objekt novyObjekt = new Objekt(aktualniID)
                //{
                //    Shape = newCircle,
                //    Popisek = popisek,
                //    ID = aktualniID++
                //};
                vsechnyObjekty.Add(novyObjekt);

                // Resetujte příznak
                pridavaniObjektu = false;
                // Resetuj barvu tlačítka
                btnImportNovyObjekt.Background = Brushes.LightGray;

                return; // vyskoč z metody, toto je vše
            }
            // Pokud je AKTIVNÍ PŘIDÁVÁNÍ VAZBY
            else if (pridavaniVazby && clickedElement is Ellipse)
            {
                // přidej pomocnou vazbu
                line0 = new Line();
                line0.Stroke = Brushes.Black;
                line0.X1 = clickPoint.X;
                line0.Y1 = clickPoint.Y;
                line0.X2 = clickPoint.X;
                line0.Y2 = clickPoint.Y;
                cnvsObjekty.Children.Add(line0);

                // Vyhledejte odpovídající objekt v seznamu vsechnyObjekty
                vybranyObjekt = vsechnyObjekty.FirstOrDefault(obj => obj.Shape == clickedElement);
                vybranyObjekt.Highlight();

                startVazbyPoint = clickPoint;//e.GetPosition(cnvsObjekty); // ulož si startovní bod Vazby
                btnImportObjektOdstranit.IsEnabled = false; // zakázat tlačítko Odstranit - není co odstraňovat

                return; // vyskoč z metody, toto je vše
            }

            // Pokud Kliknu MIMO Objekt a Vazbu:
            if (!(clickedElement is Ellipse) && !(clickedElement is Line))
            {
                OdznacObjekty();
                OdznacVazby();

                vybranyObjekt = null;
                vybranaVazba = null;

                btnImportObjektOdstranit.IsEnabled = false;

                foreach (var objekt in vsechnyObjekty)
                {
                    objekt.ResetAppearance(); // Restartuje vlastnosti všech objektů
                }
                foreach (var vazba in vsechnyVazby)
                {
                    vazba.ResetAppearance(); // Restartuje vlastnosti všech vazeb
                }

                // klikl jsem kolem vazby?
                if (GetClickedVazba(clickPoint) != null)
                {
                    clickedElement = GetClickedVazba(clickPoint).Line; // pokud jo, vrať tu přímku (vazbu)
                }

            }

            // Pokud je VYBRÁN OBJEKT (Elipsa) a NENÍ Zvoleno PŘIDÁVÁNÍ VAZBY:
            if (clickedElement is Ellipse clickedEllipse && !pridavaniVazby) //(e.OriginalSource is Ellipse clickedEllipse && !pridavaniVazby)
            {
                // Vyhledejte odpovídající objekt v seznamu vsechnyObjekty na základě kliknuté elipsy
                vybranyObjekt = vsechnyObjekty.FirstOrDefault(obj => obj.Shape == clickedEllipse);

                // Označte vybraný objekt (nějakým způsobem, např. změnou barvy)
                if (vybranyObjekt != null)
                {
                    #region OLD
                    //// pro všechny objekty v Canvas
                    //foreach (var obj in cnvsObjekty.Children)
                    //{
                    //    if (obj is Ellipse ellipse)
                    //    {
                    //        // Restartuje vlastnosti ostatních objektů:
                    //        ellipse.Fill = Brushes.White; // Resetuj barvu výplně
                    //        ellipse.Stroke = Brushes.Black; // Resetuj barvu okraje
                    //        ellipse.StrokeThickness = 1; // Resetuj tloušťku okraje
                    //                                     // Resetuj vazby
                    //        odznacVazby();
                    //    }
                    //}
                    //// Označení objektu
                    //clickedEllipse.Fill = Brushes.LightCyan;
                    //clickedEllipse.Stroke = Brushes.DodgerBlue;
                    //clickedEllipse.StrokeThickness = 3;
                    //
                    #endregion
                    //// NAHRAZENO:
                    OdznacObjekty(); // fce pro odznačení všech objektů
                    OdznacVazby();   // fce pro odznačení všech vazeb

                    vybranyObjekt.Highlight();
                    // nastav vybranou vazbu na null
                    vybranaVazba = null;
                    // povolit tlačítko odstranit
                    btnImportObjektOdstranit.IsEnabled = true;


                    presouvani = true; // Aktivujte režim přesouvání
                    posledniPozice = clickPoint;//e.GetPosition(cnvsObjekty); // Uložte aktuální pozici myši
                }
            }
            // Pokud je VYBRÁNA VAZBA:
            else if (clickedElement is Line) // pokud je vybrána vazba
            {
                // Vyhledejte odpovídající objekt v seznamu vsechnyObjekty
                vybranaVazba = vsechnyVazby.FirstOrDefault(vazba => vazba.Line == clickedElement);

                var clickedVazba = GetClickedVazba(clickPoint);

                if (clickedVazba != null)
                {
                    OdznacVazby(); // Restartuje vlastnosti všech vazeb

                    OdznacObjekty(); // Restartuje vlastnosti všech objektů

                    clickedVazba.Highlight(); // Označí kliknutou vazbu

                    vybranyObjekt = null; // nastav vybrany objekt na null

                    btnImportObjektOdstranit.IsEnabled = true; // povolit tlačítko odstranit
                }
            }

        }

        private List<Vazba> listVazbebObjektu = new List<Vazba>(); // vazby objektu

        // fce pro Nalezení Všech Vazeb Objektu
        private List<Vazba> najdiVazbyObjektu(Objekt obj) //, List<Vazba> listVazbebObjektu)
        {
            listVazbebObjektu.Clear(); // pročisti seznam

            foreach (var vazba in vsechnyVazby)
            {
                // pokud se shoduje ID obejktu vazby s ID vybraného objektu
                if (vazba.StartObjekt.ID == obj.ID || vazba.EndObjekt.ID == obj.ID)
                {
                    listVazbebObjektu.Add(vazba);
                }
            }

            return listVazbebObjektu;
        }

        // METODA PRO PŘESUN OBJEKTU PO PLÁTNĚ:
        private void cnvsObjekty_MouseMove(object sender, MouseEventArgs e)
        {
            // Získejte novou pozici myši
            Point novaPozice = e.GetPosition(cnvsObjekty);

            if (presouvani && vybranyObjekt != null)
            {
                // Vypočítejte posun
                double deltaX = novaPozice.X - posledniPozice.X;
                double deltaY = novaPozice.Y - posledniPozice.Y;

                // Aktualizujte pozici objektu (elipsy)
                double newLeft = Canvas.GetLeft(vybranyObjekt.Shape) + deltaX;
                double newTop = Canvas.GetTop(vybranyObjekt.Shape) + deltaY;
                Canvas.SetLeft(vybranyObjekt.Shape, newLeft);
                Canvas.SetTop(vybranyObjekt.Shape, newTop);

                // Aktualizujte pozici popisku, aby byl vprostřed pod elipsou
                Canvas.SetLeft(vybranyObjekt.Popisek, newLeft + vybranyObjekt.Shape.Width / 2 - (vybranyObjekt.Popisek.ActualWidth / 2));
                Canvas.SetTop(vybranyObjekt.Popisek, newTop + vybranyObjekt.Shape.Height);

                // Aktualizujte poslední pozici pro další pohyb
                posledniPozice = novaPozice;

                // Najdi všechny Vazby přesouvaného objektu
                var vazbyVybranéhObjektu = najdiVazbyObjektu(vybranyObjekt);
                foreach (var vazba in vazbyVybranéhObjektu)
                {
                    //vazba.EndPoint = novaPozice;
                    if (vybranyObjekt.ID == vazba.StartObjekt.ID)
                    {
                        vazba.StartPoint = new Point(novaPozice.X, novaPozice.Y);
                        vazba.Line.X1 = Canvas.GetLeft(vybranyObjekt.Shape) + vybranyObjekt.Shape.Width / 2;
                        vazba.Line.Y1 = Canvas.GetTop(vybranyObjekt.Shape) + vybranyObjekt.Shape.Height / 2;

                        vazba.Line.Stroke = Brushes.Black;
                    }
                    else if (vybranyObjekt.ID == vazba.EndObjekt.ID)
                    {
                        vazba.EndPoint = new Point(novaPozice.X, novaPozice.Y);
                        vazba.Line.X2 = Canvas.GetLeft(vybranyObjekt.Shape) + vybranyObjekt.Shape.Width / 2;
                        vazba.Line.Y2 = Canvas.GetTop(vybranyObjekt.Shape) + vybranyObjekt.Shape.Height / 2;
                        vazba.Line.Stroke = Brushes.Black;
                    }
                }
            }
            else if (!presouvani && startVazbyPoint != null)
            {
                // Pomocná vazba:
                line0.X2 = novaPozice.X; //Canvas.GetLeft(Line.Shape.Width // novaPozice.X);
                line0.Y2 = novaPozice.Y; //Canvas.GetTop(novaPozice.Y);
            }
        }

        // PO PUŠTĚNÍ LEVÉHO TLAČÍTKA MYŠI NA PLÁTNĚ CANVAS:
        private void cnvsObjekty_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // ukončIT režim přesouvání:
            presouvani = false; // Deaktivujte režim přesouvání
                                // aktualizuj Start a End Pointy vazby
            AktualizovatVazbyStartEndPoints();

            // Když přidávám Vazbu a je hodnota Startu_Vazby
            if (pridavaniVazby && startVazbyPoint.HasValue)
            {
                cnvsObjekty.Children.Remove(line0);
                //
                // naplň endObjekt objektem, na který se kliklo v Canvas
                Objekt endObjekt = MyHitTest(sender, e, typeof(Ellipse));
                //// Původní řešení:
                //Objekt endObjekt = vsechnyObjekty.FirstOrDefault(obj => obj.Shape == e.OriginalSource as Ellipse);

                //// Vytvoření seznamu pro uložení všech prvků, které byly detekovány
                //List<UIElement> hitObjects = new List<UIElement>();

                //// Provedení hittestingu
                //VisualTreeHelper.HitTest(
                //    sender as Canvas,
                //    null,
                //    new HitTestResultCallback(result =>
                //    {
                //        if (result.VisualHit is UIElement element && cnvsObjekty.Children.Contains(element))
                //        {
                //            hitObjects.Add(element);
                //        }
                //        return HitTestResultBehavior.Continue;
                //    }),
                //    new PointHitTestParameters(e.GetPosition((UIElement)sender))
                //);

                //// Procházení seznamu a zpracování výsledků
                //foreach (var hit in hitObjects)
                //{
                //    if (hit is Ellipse)
                //    {
                //        endObjekt = vsechnyObjekty.FirstOrDefault(obj => obj.Shape == hit);
                //        break;
                //    }
                //}

                // Pokud je vybraný objekt:
                if (vybranyObjekt != null && endObjekt != null && vybranyObjekt != endObjekt)
                {
                    //Vytvořte linku mezi objekty
                    Line line = new Line();
                    line.X1 = Canvas.GetLeft(vybranyObjekt.Shape) + vybranyObjekt.Shape.Width / 2;
                    line.Y1 = Canvas.GetTop(vybranyObjekt.Shape) + vybranyObjekt.Shape.Height / 2;
                    line.X2 = Canvas.GetLeft(endObjekt.Shape) + endObjekt.Shape.Width / 2;
                    line.Y2 = Canvas.GetTop(endObjekt.Shape) + endObjekt.Shape.Height / 2;
                    line.Stroke = Brushes.Black;
                    cnvsObjekty.Children.Add(line);

                    // Uložte vazbu
                    Vazba newVazba = new Vazba(aktualniID++, vybranyObjekt, endObjekt, line);
                    vsechnyVazby.Add(newVazba);

                    OdznacObjekty(); // odznač vybraný objekt č.1
                    btnImportObjektOdstranit.IsEnabled = false; // zakázat tlačítko Odstranit - není co odstraňovat
                }
                else
                {
                    cnvsObjekty.Children.Remove(line0);
                }

                startVazbyPoint = null;

                // obnov výchozí hodnoty
                pridavaniVazby = false;
                // Resetuj barvu tlačítka
                btnImportNovaVazba.Background = Brushes.LightGray;
            }
        }

        // FUNKCE PRO KLIKNUTÍ NA VAZBU:
        private Vazba GetClickedVazba(Point clickedPoint)
        {
            const double tolerance = 10.0; // Toto můžete upravit dle potřeby

            foreach (var vazba in vsechnyVazby)
            {
                // Detekce kliknutí na vazbu - jednoduchý přístup
                // Zde se předpokládá, že vazba je přímá čára
                var isCloseToLine = PointIsCloseToLine(clickedPoint, vazba.StartPoint, vazba.EndPoint, tolerance);
                if (isCloseToLine)
                {
                    return vazba;
                }
            }

            return null;
        }

        // Funkce pro detekci blízkosti bodu k přímce
        private bool PointIsCloseToLine(Point pt, Point lineStart, Point lineEnd, double tolerance)
        {
            var lineLength = Math.Sqrt(Math.Pow(lineEnd.X - lineStart.X, 2) + Math.Pow(lineEnd.Y - lineStart.Y, 2));

            if (lineLength == 0.0) return false;

            var distance = Math.Abs((pt.X - lineStart.X) * (lineEnd.Y - lineStart.Y) -
                                    (pt.Y - lineStart.Y) * (lineEnd.X - lineStart.X)) / lineLength;

            return distance < tolerance;
        }


        private void AktualizovatVazbyStartEndPoints()
        {
            foreach (var vazba in vsechnyVazby)
            {
                var elipsa1 = vsechnyObjekty.FirstOrDefault(obj => obj.ID == vazba.StartObjekt.ID)?.Shape;
                var elipsa2 = vsechnyObjekty.FirstOrDefault(obj => obj.ID == vazba.EndObjekt.ID)?.Shape;

                if (elipsa1 != null && elipsa2 != null)
                {
                    vazba.StartPoint = new Point(Canvas.GetLeft(elipsa1) + elipsa1.Width / 2, Canvas.GetTop(elipsa1) + elipsa1.Height / 2);
                    vazba.EndPoint = new Point(Canvas.GetLeft(elipsa2) + elipsa2.Width / 2, Canvas.GetTop(elipsa2) + elipsa2.Height / 2);
                }
            }
        }
    }
}