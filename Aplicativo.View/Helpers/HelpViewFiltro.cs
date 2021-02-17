using Aplicativo.Utils.Helpers;
using Aplicativo.View.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicativo.View.Helpers
{
    public static class HelpViewFiltro
    {

        public static HelpFiltro HelpFiltro(string Label, string Column, FiltroType FiltroType)
        {
            var HelpFiltro = new HelpFiltro()
            {
                Label = Label,
                Column = Column,
                Type = FiltroType,
            };

            if (FiltroType == FiltroType.TextBox)
            {
                HelpFiltro.Element = new object[] { new TextBox() };
            }


            return HelpFiltro;

        }
    }
}