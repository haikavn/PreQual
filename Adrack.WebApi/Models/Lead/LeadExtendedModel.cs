using System.Collections.Generic;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadExtendedModel : LeadModel
    {
        public IList<NoteTitle> AllLeadNotes { get; internal set; }
        public IList<Affiliate> AllAffiliatesList { get; internal set; }
        public IList<Buyer> AllBuyersList { get; internal set; }
        public IList<Core.Domain.Lead.BuyerChannel> AllBuyerChannelsList { get; internal set; }
        public IList<Core.Domain.Lead.AffiliateChannel> AllAffiliateChannelsList { get; internal set; }
        public IList<Campaign> AllCampaignsList { get; internal set; }
        public string VisibleColumns { get; internal set; }
        public LeadModel Lead { get; internal set; } // TODO remove
        public long AffiliateId { get; internal set; }
        public long BuyerId { get; internal set; }
        public long SelectedBuyerId { get; internal set; }
        public string AffiliateName { get; internal set; }
        public string BuyerName { get; internal set; }
        public string Type { get; internal set; }
        public List<SelectItem> ListStates { get; internal set; }
    }
}