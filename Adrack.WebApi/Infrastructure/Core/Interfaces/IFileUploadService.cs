using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Infrastructure.Core.Interfaces
{
    public interface IFileUploadService
    {
        string UploadFile(long id);
        bool UploadLogoFile(string fileName, long id, out string errMessage);
        void DeleteFile(BuyerChannel buyerChannel, HttpPostedFileBase file);
    }
}
