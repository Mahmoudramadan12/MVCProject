using Demo.DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace DemoPL.Helpers.Interfaces
{
	public interface ISmsService
	{

		public MessageResource SendSms(SmsMessage sms);

	}
}
