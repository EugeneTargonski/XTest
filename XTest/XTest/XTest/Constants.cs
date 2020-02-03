using System;
using System.Collections.Generic;
using System.Text;

namespace XTest
{
	public static class Constants
	{
		public static string AppName = "OAuthNativeFlow";

		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "<insert IOS client ID here>";
		public static string AndroidClientId = "21170702167-jriifs3n881an76deo58c5lb92l6fj11.apps.googleusercontent.com";

		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.profile";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string iOSRedirectUrl = "<insert IOS redirect URL here>:/oauth2redirect";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.21170702167-jriifs3n881an76deo58c5lb92l6fj11:/oauth2redirect";
	}
}
