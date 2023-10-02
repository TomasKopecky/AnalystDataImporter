using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        // příznak pro přidávání objektu
        private bool pridavaniObjektu = false;
        // příznak pro přidávání objektu
        private bool pridavaniVazby = false;
        // příznak pro odebírání objektu
        private bool odebiraniObejktu = false;

        // příznak pro stisklé pravé tlačítko myši
        private bool isRightMouseButtonDown = false;
        private Point previousMousePosition;

        // přízna pro ZOOM
        private double zoomFactor = 1.1; // Faktor zoomu


        private void btnImportNovyObjekt_Click(object sender, RoutedEventArgs e)
        {
            // Nastavte příznak pro přidávání objektu
            pridavaniObjektu = true;
            pridavaniVazby = false;
            odebiraniObejktu = false;

            // tlačítko bude svítit modře
            btnImportNovyObjekt.Background = Brushes.LightSkyBlue;
            // Resetuj barvu tlačítka
            btnImportNovaVazba.Background = Brushes.LightGray;
            // Resetuj barvu tlačítka
            btnImportObjektOdstranit.Background = Brushes.LightGray;
        }

        private void btnImportNovaVazba_Click(object sender, RoutedEventArgs e)
        {
            // Nastavte příznak pro přidávání vazby
            pridavaniVazby = true;
            pridavaniObjektu = false;
            odebiraniObejktu = false;

            // tlačítko bude svítit modře
            btnImportNovaVazba.Background = Brushes.LightSkyBlue;
            // Resetuj barvu tlačítka
            btnImportNovyObjekt.Background = Brushes.LightGray;
            // Resetuj barvu tlačítka
            btnImportObjektOdstranit.Background = Brushes.LightGray;
        }

        private void btnImportObjektOdstranit_Click(object sender, RoutedEventArgs e)
        {
            // Nastavte režim pro odebrání objektu po kliknutí
            odebiraniObejktu = true;
            pridavaniObjektu = false;
            pridavaniVazby = false;

            // tlačítko bude svítit modře
            btnImportObjektOdstranit.Background = Brushes.LightSkyBlue; //SkyBlue;
            // Resetuj barvu tlačítka
            btnImportNovyObjekt.Background = Brushes.LightGray;
            // Resetuj barvu tlačítka
            btnImportNovaVazba.Background = Brushes.LightGray;
        }

        private void cnvsObjekty_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (pridavaniObjektu)
            {
                // Získání pozice kliknutí myší
                Point position = e.GetPosition(cnvsObjekty);

                // Přidejte žlutý kruh do Canvas na pozici kliknutí
                Ellipse newCircle = new Ellipse();
                newCircle.Fill = Brushes.White;
                newCircle.Stroke = Brushes.Black;
                newCircle.Width = 50;
                newCircle.Height = 50;

                // Nastavte pozici kruhu na pozici kliknutí
                Canvas.SetLeft(newCircle, position.X - newCircle.Width / 2);
                Canvas.SetTop(newCircle, position.Y - newCircle.Height / 2);

                // Přidejte kruh do Canvas
                cnvsObjekty.Children.Add(newCircle);

                // Resetujte příznak
                pridavaniObjektu = false;
                // Resetuj barvu tlačítka
                btnImportNovyObjekt.Background = Brushes.LightGray;
            }
            else if (odebiraniObejktu)
            {
                // Pokud nejste v režimu přidávání objektu, zjistěte, zda byl kliknutý objekt
                UIElement clickedElement = e.OriginalSource as UIElement;

                // Odeberte kliknutý objekt z plátna (Canvas)
                if (clickedElement != null && cnvsObjekty.Children.Contains(clickedElement))
                {
                    cnvsObjekty.Children.Remove(clickedElement);
                }
                // Resetujte příznak
                odebiraniObejktu = false;
                // Resetuj barvu tlačítka
                btnImportObjektOdstranit.Background = Brushes.LightGray;
            }
        }

        // stiskl jsem pravé tlačítko myši
        private void cnvsObjekty_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            isRightMouseButtonDown = true;
            previousMousePosition = e.GetPosition(cnvsObjekty);
        }
        // hýbu myší
        private void cnvsObjekty_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isRightMouseButtonDown)
            {
                Point currentMousePosition = e.GetPosition(cnvsObjekty);
                double deltaX = currentMousePosition.X - previousMousePosition.X;
                double deltaY = currentMousePosition.Y - previousMousePosition.Y;

                // Posuňte plátno podle změny pozice myši
                Canvas.SetLeft(cnvsObjekty, Canvas.GetLeft(cnvsObjekty) + deltaX);
                Canvas.SetTop(cnvsObjekty, Canvas.GetTop(cnvsObjekty) + deltaY);

                previousMousePosition = currentMousePosition;
            }
        }
        // pustil jsem pravé tlačítko myši
        private void cnvsObjekty_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            isRightMouseButtonDown = false;
        }

        private void cnvsObjekty_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (e.Delta > 0)
            //{
            //    // Přibližte plátno
            //    cnvsObjekty.LayoutTransform = new ScaleTransform(cnvsObjekty.LayoutTransform.Value.M11 * zoomFactor, cnvsObjekty.LayoutTransform.Value.M22 * zoomFactor);
            //}
            //else
            //{
            //    // Oddalte plátno
            //    cnvsObjekty.LayoutTransform = new ScaleTransform(cnvsObjekty.LayoutTransform.Value.M11 / zoomFactor, cnvsObjekty.LayoutTransform.Value.M22 / zoomFactor);
            //}

            Point position = e.GetPosition(cnvsObjekty);

            double zoomFactor = e.Delta > 0 ? 1.1 : 0.9; // Zvětšení nebo zmenšení faktor

            // Nastavte nový zoom faktor
            ScaleTransform scaleTransform = cnvsObjekty.LayoutTransform as ScaleTransform;
            if (scaleTransform == null)
            {
                scaleTransform = new ScaleTransform();
                cnvsObjekty.LayoutTransform = scaleTransform;
            }

            scaleTransform.CenterX = position.X;
            scaleTransform.CenterY = position.Y;
            scaleTransform.ScaleX *= zoomFactor;
            scaleTransform.ScaleY *= zoomFactor;
        }
    }
}
