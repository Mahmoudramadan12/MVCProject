using Demo.DAL.Models;
using DemoPL.Helpers.Interfaces;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace DemoPL.Helpers
{
	public class SmsService(IOptions<TwilioSettings> _options) : ISmsService
	{
		public MessageResource SendSms(SmsMessage sms)
		{
			TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);
			var result = MessageResource.Create(
				body: sms.Body,
				from: new Twilio.Types.PhoneNumber(_options.Value.TwilioPhoneNumber),
				to: sms.PhoneNumber);
			return result;
		}

	}
}
