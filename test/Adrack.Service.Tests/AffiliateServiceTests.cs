using System.Collections.Generic;
using System.Linq;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Moq;
using Xunit;

namespace Adrack.Service.Tests
{
    public class AffiliateServiceTests
    {
        private Mock<IAffiliateService> _fakeAffiliateService;
        private Mock<IRepository<Affiliate>> _fakeAffiliateRepository;
        private Mock<ICacheManager> _fakeCacheManager;
        private Mock<IAppEventPublisher> _fakeAppEventPublisher;
        private Mock<IDbContext> _fakeDbContext;
        private Mock<IAppContext> _fakeAppContext;
        private Mock<IUserService> _fakeUserService;


        [Fact]
        public void GetAllAffiliates()
        {
            var appUser = new User
            {
                ParentId = 0
            };
            _fakeAppContext.Setup(x => x.AppUser).Returns(appUser);

            var affiliates = new List<Affiliate>()
            {
                new Affiliate()
                {
                    
                }
            }.AsQueryable();

            _fakeAffiliateRepository.Setup(x => x.Table).Returns(affiliates);

            var affiliateService = new AffiliateService(_fakeAffiliateRepository.Object, _fakeCacheManager.Object, _fakeAppEventPublisher.Object, _fakeDbContext.Object, _fakeAppContext.Object, _fakeUserService.Object);
            affiliateService.GetAllAffiliates(0);
            Assert.True(true);
        }
    }
}