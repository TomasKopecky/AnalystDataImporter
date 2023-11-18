﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Factories
{
    /// <summary>
    /// Rozhraní definující metodu pro vytváření ViewModelů prvků. Rozhraní specifikuje, jaké metody a vlastnosti třída, která toto rozhraní implementuje, musí mít.
    /// </summary>
    public interface IElementViewModelFactory
    {
        /// <summary>
        /// Metoda pro vytváření nové instance ViewModel pro prvek.
        /// </summary>
        /// <returns>Nová instance třídy ElementViewModel.</returns>
        ElementViewModel Create();
    }
}