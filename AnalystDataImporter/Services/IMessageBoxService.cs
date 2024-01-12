using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Services
{
    public interface IMessageBoxService
    {
        void ShowInformation(string message);
        void ShowError(string message);
    }
}
