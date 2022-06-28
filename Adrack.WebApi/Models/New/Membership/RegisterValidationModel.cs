namespace Adrack.WebApi.Models.New.Membership
{
    public class RegisterValidationModel
    {
        public bool Resend { get; set; }
        public string Email { get; set; }
        public string ValidationCode { get; set; }
        public string Name { get; set; }
    }
}