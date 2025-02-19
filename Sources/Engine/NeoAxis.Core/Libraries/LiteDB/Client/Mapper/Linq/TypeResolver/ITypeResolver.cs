﻿#if !NO_LITE_DB
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static Internal.LiteDB.Constants;

namespace Internal.LiteDB
{
    internal interface ITypeResolver
    {
        string ResolveMethod(MethodInfo method);

        string ResolveMember(MemberInfo member);

        string ResolveCtor(ConstructorInfo ctor);
    }
}
#endif