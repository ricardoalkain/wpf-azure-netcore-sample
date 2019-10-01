using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace TTMS.Data.Tests.Mocks
{
    /// <summary>
    /// This class allows to mock a <see cref="CloudTable"/> object defining
    /// a default constructor required by Moq and NSubstitute libraries
    /// </summary>
    public class CloudTableMock : CloudTable
    {
        public CloudTableMock() : base(new Uri("http://127.0.0.1/fake/table"))
        {
        }
    }
}
