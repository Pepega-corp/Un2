using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Infrastructure.Functional
{
    public struct Nothing : IEquatable<Nothing>, IResult
    {
        public override int GetHashCode() => 0;

        public static bool operator ==(Nothing left, Nothing right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Nothing left, Nothing right)
        {
            return !Equals(left, right);
        }

        public bool HasValue => false;

        public object Value
        {
            get { throw new NotSupportedException(); }
        }

        public bool Equals(Nothing other) => true;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Nothing || obj.MaybeAs<Result>().IsSuccess;
        }

        public override string ToString() => "Nothing";
    }
}
