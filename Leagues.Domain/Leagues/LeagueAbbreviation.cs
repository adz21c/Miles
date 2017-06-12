using System;
using System.Text.RegularExpressions;

namespace Leagues.Domain.Leagues
{
    public class LeagueAbbreviation
    {
        private static readonly Regex Valid = new Regex("[a-z]+", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public static bool operator ==(LeagueAbbreviation a, LeagueAbbreviation b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(LeagueAbbreviation a, LeagueAbbreviation b)
        {
            return !(a == b);
        }

        public static LeagueAbbreviation Parse(string abbr)
        {
            LeagueAbbreviation leagueAbbr;
            if (!TryParse(abbr, out leagueAbbr))
                throw new FormatException("The supplied abbreviation is not correctly formatted.");

            return leagueAbbr;
        }

        public static bool TryParse(string abbr, out LeagueAbbreviation leagueAbbr)
        {
            leagueAbbr = null;
            if (!Valid.IsMatch(abbr))
                return false;

            leagueAbbr = new LeagueAbbreviation(abbr);
            return true;
        }

        protected LeagueAbbreviation()
        { }

        private LeagueAbbreviation(string abbreviation)
        {
            if (!Valid.IsMatch(abbreviation))
                throw new ArgumentOutOfRangeException("abbreviation");

            this.Abbreviation = abbreviation;
        }

        public string Abbreviation { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var otherObj = obj as LeagueAbbreviation;
            if (otherObj == null)
                return false;

            return this.Abbreviation.Equals(otherObj.Abbreviation);
        }

        public override int GetHashCode()
        {
            return this.Abbreviation.GetHashCode();
        }

        public override string ToString()
        {
            return this.Abbreviation;
        }
    }
}
