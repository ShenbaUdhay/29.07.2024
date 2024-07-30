namespace Modules
{
    public static class CriteriaString
    {
        public const string CurrentCompanyOrAdministrative = "Oid = CurrentCompanyOid() OR IsAdministrative() = '1'";
        public const string CurrentCompany = "Oid = CurrentCompanyOid()";
        public const string IsAdministrative = "IsAdministrative() = '1'";
        public const string IsNotAdministrative = "IsAdministrative() <> '1'";
    }
}
