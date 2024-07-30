using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.Setting.SDMS
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SpreadSheetBuilder_ScientificData : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SpreadSheetBuilder_ScientificData(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        int fuqID;
        [Key(true)]
        public int uqID
        {
            get { return fuqID; }
            set { SetPropertyValue<int>(nameof(uqID), ref fuqID, value); }
        }

        string fResultType;
        [Size(50)]
        public string ResultType
        {
            get { return fResultType; }
            set { SetPropertyValue<string>(nameof(ResultType), ref fResultType, value); }
        }
        string fFieldName;
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue]
        [ImmediatePostData]
        public string FieldName
        {
            get { return fFieldName; }
            set { SetPropertyValue<string>(nameof(FieldName), ref fFieldName, value); }
        }
        string fSynonym_EN;
        public string Synonym_EN
        {
            get { return fSynonym_EN; }
            set { SetPropertyValue<string>(nameof(Synonym_EN), ref fSynonym_EN, value); }
        }
        string fSynonym_CN;
        public string Synonym_CN
        {
            get { return fSynonym_CN; }
            set { SetPropertyValue<string>(nameof(Synonym_CN), ref fSynonym_CN, value); }
        }
        string fDataType;
        //[EditorAlias("StringTestComoboxPropertyEditor")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string DataType
        {
            get { return fDataType; }
            set { SetPropertyValue<string>(nameof(DataType), ref fDataType, value); }
        }

        private ScientificDataTypes _NonPersistantDataType;
        [NonPersistent]
        [ImmediatePostData]
        public ScientificDataTypes NonPersistantDataType
        {
            get
            {
                if (!string.IsNullOrEmpty(DataType))
                {

                    if (DataType == "bigint")
                    {
                        _NonPersistantDataType = ScientificDataTypes.bigint;
                    }
                    else if (DataType == "numeric")
                    {
                        _NonPersistantDataType = ScientificDataTypes.numeric;
                    }
                    else if (DataType == "bit")
                    {
                        _NonPersistantDataType = ScientificDataTypes.bit;
                    }
                    else if (DataType == "smallint")
                    {
                        _NonPersistantDataType = ScientificDataTypes.smallint;
                    }
                    else if (DataType == "decimal")
                    {
                        _NonPersistantDataType = ScientificDataTypes.decimaltype;
                    }
                    else if (DataType == "smallmoney")
                    {
                        _NonPersistantDataType = ScientificDataTypes.smallmoney;
                    }
                    else if (DataType == "int")
                    {
                        _NonPersistantDataType = ScientificDataTypes.inttype;
                    }
                    else if (DataType == "tinyint")
                    {
                        _NonPersistantDataType = ScientificDataTypes.tinyint;
                    }
                    else if (DataType == "money")
                    {
                        _NonPersistantDataType = ScientificDataTypes.money;
                    }
                    else if (DataType == "float")
                    {
                        _NonPersistantDataType = ScientificDataTypes.floattype;
                    }
                    else if (DataType == "real")
                    {

                        _NonPersistantDataType = ScientificDataTypes.real;
                    }
                    else if (DataType == "date")
                    {
                        _NonPersistantDataType = ScientificDataTypes.date;
                    }
                    else if (DataType == "datetime2")
                    {
                        _NonPersistantDataType = ScientificDataTypes.datetime2;
                    }
                    else if (DataType == "datetime")
                    {
                        _NonPersistantDataType = ScientificDataTypes.datetime;
                    }
                    else if (DataType == "datetimeoffset")
                    {
                        _NonPersistantDataType = ScientificDataTypes.datetimeoffset;
                    }
                    else if (DataType == "smalldatetime")
                    {
                        _NonPersistantDataType = ScientificDataTypes.smalldatetime;
                    }
                    else if (DataType == "time")
                    {
                        _NonPersistantDataType = ScientificDataTypes.time;
                    }
                    else if (DataType == "char")
                    {
                        _NonPersistantDataType = ScientificDataTypes.chartype;
                    }
                    else if (DataType == "varchar(10)")
                    {
                        _NonPersistantDataType = ScientificDataTypes.varchar10;
                    }
                    else if (DataType == "varchar(50)")
                    {
                        _NonPersistantDataType = ScientificDataTypes.varchar50;
                    }
                    else if (DataType == "varchar(100)")
                    {
                        _NonPersistantDataType = ScientificDataTypes.varchar100;
                    }
                    else if (DataType == "text")
                    {
                        _NonPersistantDataType = ScientificDataTypes.text;
                    }
                    else if (DataType == "nchar")
                    {
                        _NonPersistantDataType = ScientificDataTypes.nchar;
                    }
                    else if (DataType == "nvarchar(10)")
                    {
                        _NonPersistantDataType = ScientificDataTypes.nvarchar10;
                    }
                    else if (DataType == "nvarchar(50)")
                    {
                        _NonPersistantDataType = ScientificDataTypes.nvarchar50;
                    }
                    else if (DataType == "nvarchar(100)")
                    {
                        _NonPersistantDataType = ScientificDataTypes.nvarchar100;
                    }
                    else if (DataType == "ntext")
                    {
                        _NonPersistantDataType = ScientificDataTypes.ntext;
                    }
                    else if (DataType == "binary")
                    {
                        _NonPersistantDataType = ScientificDataTypes.binary;
                    }
                    else if (DataType == "varbinary")
                    {
                        _NonPersistantDataType = ScientificDataTypes.varbinary;
                    }
                    else if (DataType == "image")
                    {
                        _NonPersistantDataType = ScientificDataTypes.image;
                    }
                    else if (DataType == "xml")
                    {
                        _NonPersistantDataType = ScientificDataTypes.xml;
                    }
                    else if (DataType == "timestamp")
                    {
                        _NonPersistantDataType = ScientificDataTypes.timestamp;
                    }

                }
                //else
                //  {
                //    _NonPersistantDataType = ScientificDataTypes.None;
                //  }

                return _NonPersistantDataType;
            }
            set { SetPropertyValue<ScientificDataTypes>(nameof(NonPersistantDataType), ref _NonPersistantDataType, value); }
        }

        string fDataSize;
        public string DataSize
        {
            get { return fDataSize; }
            set { SetPropertyValue<string>(nameof(DataSize), ref fDataSize, value); }
        }
        string fFieldNo;
        public string FieldNo
        {
            get { return fFieldNo; }
            set { SetPropertyValue<string>(nameof(FieldNo), ref fFieldNo, value); }
        }
        string fDescription;
        [Size(1000)]
        public string Description
        {
            get { return fDescription; }
            set { SetPropertyValue<string>(nameof(Description), ref fDescription, value); }
        }
        bool fCommon;
        public bool Common
        {
            get { return fCommon; }
            set { SetPropertyValue<bool>(nameof(Common), ref fCommon, value); }
        }
        bool fSaveInTable;
        //[ImmediatePostData]
        public bool SaveInTable
        {
            get
            {
                return fSaveInTable;
            }
            set { SetPropertyValue<bool>(nameof(SaveInTable), ref fSaveInTable, value); }
        }

        private ScientificDataAction fAction;
        [NonPersistent]
        public ScientificDataAction Action
        {
            get
            {
                return fAction;
            }
            set { SetPropertyValue<ScientificDataAction>(nameof(Action), ref fAction, value); }
        }

        private string fOldColumnName;
        [NonPersistent]
        public string OldColumnName
        {
            get
            {
                return fOldColumnName;
            }
            set
            {
                SetPropertyValue<string>(nameof(OldColumnName), ref fOldColumnName, value);
            }
        }

        private int fSort;
        //  [NonPersistent]
        public int Sort
        {
            get
            {
                return fSort;
            }
            set
            {

                SetPropertyValue<int>(nameof(Sort), ref fSort, value);

            }
        }

    }
}