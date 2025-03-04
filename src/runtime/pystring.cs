using System;

namespace Python.Runtime
{
    /// <summary>
    /// Represents a Python (ANSI) string object. See the documentation at
    /// PY2: https://docs.python.org/2/c-api/string.html
    /// PY3: No Equivalent
    /// for details.
    /// </summary>
    /// <remarks>
    /// 2011-01-29: ...Then why does the string constructor call PyUnicode_FromUnicode()???
    /// </remarks>
    public class PyString : PySequence
    {
        internal PyString(in StolenReference reference) : base(reference) { }
        internal PyString(BorrowedReference reference) : base(reference) { }


        private static BorrowedReference FromObject(PyObject o)
        {
            if (o is null) throw new ArgumentNullException(nameof(o));
            if (!IsStringType(o))
            {
                throw new ArgumentException("object is not a string");
            }
            return o.Reference;
        }

        /// <summary>
        /// PyString Constructor
        /// </summary>
        /// <remarks>
        /// Copy constructor - obtain a PyString from a generic PyObject.
        /// An ArgumentException will be thrown if the given object is not
        /// a Python string object.
        /// </remarks>
        public PyString(PyObject o) : base(FromObject(o))
        {
        }


        private static NewReference FromString(string s)
        {
            IntPtr val = Runtime.PyString_FromString(s);
            PythonException.ThrowIfIsNull(val);
            return NewReference.DangerousFromPointer(val);
        }
        /// <summary>
        /// PyString Constructor
        /// </summary>
        /// <remarks>
        /// Creates a Python string from a managed string.
        /// </remarks>
        public PyString(string s) : base(FromString(s).Steal())
        {
        }


        /// <summary>
        /// Returns true if the given object is a Python string.
        /// </summary>
        public static bool IsStringType(PyObject value)
        {
            return Runtime.PyString_Check(value.obj);
        }
    }
}
