using System;
using System.Text.RegularExpressions;

namespace Leagues.Domain.Teams
{
    public class TeamAbbreviation
    {
        private static readonly Regex Valid = new Regex("[a-z]+", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public static bool operator ==(TeamAbbreviation a, TeamAbbreviation b)
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

        public static bool operator !=(TeamAbbreviation a, TeamAbbreviation b)
        {
            return !(a == b);
        }

        public static TeamAbbreviation Parse(string abbr)
        {
            TeamAbbreviation teamAbbr;
            if (!TryParse(abbr, out teamAbbr))
                throw new FormatException("The supplied abbreviation is not correctly formatted.");

            return teamAbbr;
        }

        public static bool TryParse(string abbr, out TeamAbbreviation teamAbbr)
        {
            teamAbbr = null;
            if (!Valid.IsMatch(abbr))
                return false;

            teamAbbr = new TeamAbbreviation(abbr);
            return true;
        }

        protected TeamAbbreviation()
        { }

        public TeamAbbreviation(string abbreviation)
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

            var otherObj = obj as TeamAbbreviation;
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
