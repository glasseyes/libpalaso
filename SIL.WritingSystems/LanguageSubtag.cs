﻿using System.Collections.Generic;

namespace SIL.WritingSystems
{
	/// <summary>
	/// This class represents a language from the IANA language subtag registry.
	/// </summary>
	public class LanguageSubtag : Subtag
	{
		private readonly string _iso3Code;
		private readonly List<string> _descriptions = new List<string>();
		private bool _isMacroLanguage = false;
		private bool _isDeprecated = false;

		/// <summary>
		/// Initializes a new private-use instance of the <see cref="LanguageSubtag"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="name">The name.</param>
		public LanguageSubtag(string code, string name = null)
			: base(code, name, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:LanguageSubtag"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="name">The name.</param>
		/// <param name="isPrivateUse">if set to <c>true</c> this is a private use subtag.</param>
		/// <param name="iso3Code">The ISO 639-3 language code.</param>
		/// <param name="descriptions">A list of names or descriptions of the subtag.</param>
		/// <param name="isMacroLanguage">if set to <c>true</c> this is a macrolanguage.</param>
		/// <param name="isDeprecated">if set to <c>true</c> this subtag is deprecated and should not be used.</param>
		internal LanguageSubtag(string code, string name, bool isPrivateUse, string iso3Code, List<string> descriptions, bool isMacroLanguage, bool isDeprecated)
			: base(code, name, isPrivateUse)
		{
			_iso3Code = iso3Code;
			_descriptions = descriptions;
			_isMacroLanguage = isMacroLanguage;
			_isDeprecated = isDeprecated;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:LanguageSubtag"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="name">The name.</param>
		/// <param name="isPrivateUse">if set to <c>true</c> this is a private use subtag.</param>
		/// <param name="iso3Code">The ISO 639-3 language code.</param>
		internal LanguageSubtag(string code, string name, bool isPrivateUse, string iso3Code)
			: base(code, name, isPrivateUse)
		{
			_iso3Code = iso3Code;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:LanguageSubtag"/> class.
		/// </summary>
		/// <param name="subtag">The subtag.</param>
		/// <param name="name">The name.</param>
		public LanguageSubtag(LanguageSubtag subtag, string name)
			: this(subtag.Code, name, subtag.IsPrivateUse, subtag._iso3Code)
		{
		}

		/// <summary>
		/// Gets the list of language names.
		/// </summary>
		/// <value>The list of language names.</value>
		public IList<string> Names
		{
			get { return _descriptions; }
		}

		/// <summary>
		/// Gets the ISO 639-3 language code.
		/// </summary>
		/// <value>The ISO 639-3 language code.</value>
		public string Iso3Code
		{
			get { return _iso3Code ?? string.Empty; }
		}

		/// <summary>
		/// Gets a value indicating whether this is a macrolanguage.
		/// </summary>
		/// <c>true</c> if this is a macrolanguage; otherwise, <c>false</c>.
		public bool IsMacroLanguage
		{
			get { return _isMacroLanguage; }
		}

		/// <summary>
		/// Gets a value indicating whether this tag is deprecated.
		/// </summary>
		/// <c>true</c> if this tag is deprecated and should not be used; otherwise, <c>false</c>.
		public bool IsDeprecated
		{
			get { return _isDeprecated; }
		}

		public static implicit operator LanguageSubtag(string code)
		{
			if (string.IsNullOrEmpty(code))
				return null;

			LanguageSubtag subtag;
			if (!StandardSubtags.RegisteredLanguages.TryGet(code, out subtag))
				subtag = new LanguageSubtag(code);
			return subtag;
		}
	}
}
