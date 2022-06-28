using System;
using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Adrack.WebApi.Tests
{
    public class UnitTestsResolveAllControllers
    {
        [Fact]
        public void ResolveAllControllers()
        {
        }

        private ITestOutputHelper _output = new TestOutputHelper();

        public IAppContext AppContext { get; set; }

        [Fact]
        public void InitTestingUser()
        {
            //AppContext = AppEngineContext.Current.Resolve<IAppContext>();

            //AppContext.AppTestingUser = new User {Email = "testingUser@adrack.com", Id = 2000020000};
        }
    }
}
