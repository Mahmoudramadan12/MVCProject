using Demo.DAL.Models;

namespace DemoPL.Helpers.Interfaces
{
	public interface IMailSettings
	{

		public void SendEmail(Email email);
	}
}
