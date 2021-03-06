// Copyright (c) 2013 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Win32;
using SIL.Keyboarding;

namespace SIL.Windows.Forms.Keyboarding.Windows
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Keyboard description for a Windows system keyboard
	/// </summary>
	/// <remarks>Holds information about a specific keyboard, especially for IMEs (e.g. whether
	/// English input mode is selected) in addition to the default keyboard description. This
	/// is necessary to restore the current setting when switching between fields with
	/// differing keyboards. The user expects that a keyboard keeps its state between fields.
	/// </remarks>
	/// ----------------------------------------------------------------------------------------
	[SuppressMessage("Gendarme.Rules.Design", "TypesWithNativeFieldsShouldBeDisposableRule",
		Justification = "WindowHandle is a reference to a control")]
	internal class WinKeyboardDescription : KeyboardDescription
	{
		private string _localizedName;
		private readonly bool _useNfcContext;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:SIL.Windows.Forms.Keyboarding.Windows.WinKeyboardDescription"/> class.
		/// </summary>
		internal WinKeyboardDescription(string id, string name, string layout, string locale, bool isAvailable,
			IInputLanguage inputLanguage, WinKeyboardAdaptor engine, string localizedName, TfInputProcessorProfile profile)
			: base(id, name, layout, locale, isAvailable, engine)
		{
			InputLanguage = inputLanguage;
			_localizedName = localizedName;
			InputProcessorProfile = profile;
			ConversionMode = (int) (Win32.IME_CMODE.NATIVE | Win32.IME_CMODE.SYMBOL);
			_useNfcContext = !IsKeymanKeyboard(profile);
		}

		private static bool IsKeymanKeyboard(TfInputProcessorProfile profile)
		{
			if (profile.ProfileType != TfProfileType.InputProcessor)
				return false;

			// check the default key value for profile.ClsId.
			var subKey = string.Format(@"CLSID\{{{0}}}", profile.ClsId);
			using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(subKey))
			{
				if (key == null)
					return false;
				var value = key.GetValue(null) as string;
				return value != null && value.Contains("Keyman");
			}
		}

		/// <summary>
		/// Indicates whether we should pass NFC or NFD data to the keyboard. This implementation
		/// returns <c>false</c> for Keyman keyboards and <c>true</c> for other keyboards.
		/// </summary>
		public override bool UseNfcContext { get { return _useNfcContext; } }

		/// <summary>
		/// Gets a localized human-readable name of the input language.
		/// </summary>
		public override string LocalizedName
		{
			get { return _localizedName; }
		}

		internal int ConversionMode { get; set; }
		internal int SentenceMode { get; set; }
		internal IntPtr WindowHandle { get; set; }

		internal TfInputProcessorProfile InputProcessorProfile { get; set; }

		internal void SetIsAvailable(bool isAvailable)
		{
			IsAvailable = isAvailable;
		}

		internal void SetLocalizedName(string localizedName)
		{
			_localizedName = localizedName;
		}
	}
}
