namespace Abp.Owin.Authentication
{
    public interface ITicketInfoProtector
    {
        string Protect(TicketInfo ticket);

        TicketInfo UnProtect(string token);
    }
}
