﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class DatasetPatrols : DataSet {
        
        private PatrolsDataTable tablePatrols;
        
        public DatasetPatrols() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected DatasetPatrols(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["Patrols"] != null)) {
                    this.Tables.Add(new PatrolsDataTable(ds.Tables["Patrols"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public PatrolsDataTable Patrols {
            get {
                return this.tablePatrols;
            }
        }
        
        public override DataSet Clone() {
            DatasetPatrols cln = ((DatasetPatrols)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["Patrols"] != null)) {
                this.Tables.Add(new PatrolsDataTable(ds.Tables["Patrols"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        internal void InitVars() {
            this.tablePatrols = ((PatrolsDataTable)(this.Tables["Patrols"]));
            if ((this.tablePatrols != null)) {
                this.tablePatrols.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "DatasetPatrols";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/DatasetPatrols.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tablePatrols = new PatrolsDataTable();
            this.Tables.Add(this.tablePatrols);
        }
        
        private bool ShouldSerializePatrols() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void PatrolsRowChangeEventHandler(object sender, PatrolsRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class PatrolsDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnId;
            
            private DataColumn columnStarttime;
            
            private DataColumn columnClass;
            
            private DataColumn columnNumberOfCompetitors;
            
            private DataColumn columnDisplayName;
            
            internal PatrolsDataTable() : 
                    base("Patrols") {
                this.InitClass();
            }
            
            internal PatrolsDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn IdColumn {
                get {
                    return this.columnId;
                }
            }
            
            internal DataColumn StarttimeColumn {
                get {
                    return this.columnStarttime;
                }
            }
            
            internal DataColumn ClassColumn {
                get {
                    return this.columnClass;
                }
            }
            
            internal DataColumn NumberOfCompetitorsColumn {
                get {
                    return this.columnNumberOfCompetitors;
                }
            }
            
            internal DataColumn DisplayNameColumn {
                get {
                    return this.columnDisplayName;
                }
            }
            
            public PatrolsRow this[int index] {
                get {
                    return ((PatrolsRow)(this.Rows[index]));
                }
            }
            
            public event PatrolsRowChangeEventHandler PatrolsRowChanged;
            
            public event PatrolsRowChangeEventHandler PatrolsRowChanging;
            
            public event PatrolsRowChangeEventHandler PatrolsRowDeleted;
            
            public event PatrolsRowChangeEventHandler PatrolsRowDeleting;
            
            public void AddPatrolsRow(PatrolsRow row) {
                this.Rows.Add(row);
            }
            
            public PatrolsRow AddPatrolsRow(int Id, System.DateTime Starttime, string Class, int NumberOfCompetitors, string DisplayName) {
                PatrolsRow rowPatrolsRow = ((PatrolsRow)(this.NewRow()));
                rowPatrolsRow.ItemArray = new object[] {
                        Id,
                        Starttime,
                        Class,
                        NumberOfCompetitors,
                        DisplayName};
                this.Rows.Add(rowPatrolsRow);
                return rowPatrolsRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                PatrolsDataTable cln = ((PatrolsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new PatrolsDataTable();
            }
            
            internal void InitVars() {
                this.columnId = this.Columns["Id"];
                this.columnStarttime = this.Columns["Starttime"];
                this.columnClass = this.Columns["Class"];
                this.columnNumberOfCompetitors = this.Columns["NumberOfCompetitors"];
                this.columnDisplayName = this.Columns["DisplayName"];
            }
            
            private void InitClass() {
                this.columnId = new DataColumn("Id", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnId);
                this.columnStarttime = new DataColumn("Starttime", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnStarttime);
                this.columnClass = new DataColumn("Class", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnClass);
                this.columnNumberOfCompetitors = new DataColumn("NumberOfCompetitors", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnNumberOfCompetitors);
                this.columnDisplayName = new DataColumn("DisplayName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDisplayName);
            }
            
            public PatrolsRow NewPatrolsRow() {
                return ((PatrolsRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new PatrolsRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(PatrolsRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.PatrolsRowChanged != null)) {
                    this.PatrolsRowChanged(this, new PatrolsRowChangeEvent(((PatrolsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.PatrolsRowChanging != null)) {
                    this.PatrolsRowChanging(this, new PatrolsRowChangeEvent(((PatrolsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.PatrolsRowDeleted != null)) {
                    this.PatrolsRowDeleted(this, new PatrolsRowChangeEvent(((PatrolsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.PatrolsRowDeleting != null)) {
                    this.PatrolsRowDeleting(this, new PatrolsRowChangeEvent(((PatrolsRow)(e.Row)), e.Action));
                }
            }
            
            public void RemovePatrolsRow(PatrolsRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class PatrolsRow : DataRow {
            
            private PatrolsDataTable tablePatrols;
            
            internal PatrolsRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tablePatrols = ((PatrolsDataTable)(this.Table));
            }
            
            public int Id {
                get {
                    try {
                        return ((int)(this[this.tablePatrols.IdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePatrols.IdColumn] = value;
                }
            }
            
            public System.DateTime Starttime {
                get {
                    try {
                        return ((System.DateTime)(this[this.tablePatrols.StarttimeColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePatrols.StarttimeColumn] = value;
                }
            }
            
            public string Class {
                get {
                    try {
                        return ((string)(this[this.tablePatrols.ClassColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePatrols.ClassColumn] = value;
                }
            }
            
            public int NumberOfCompetitors {
                get {
                    try {
                        return ((int)(this[this.tablePatrols.NumberOfCompetitorsColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePatrols.NumberOfCompetitorsColumn] = value;
                }
            }
            
            public string DisplayName {
                get {
                    try {
                        return ((string)(this[this.tablePatrols.DisplayNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePatrols.DisplayNameColumn] = value;
                }
            }
            
            public bool IsIdNull() {
                return this.IsNull(this.tablePatrols.IdColumn);
            }
            
            public void SetIdNull() {
                this[this.tablePatrols.IdColumn] = System.Convert.DBNull;
            }
            
            public bool IsStarttimeNull() {
                return this.IsNull(this.tablePatrols.StarttimeColumn);
            }
            
            public void SetStarttimeNull() {
                this[this.tablePatrols.StarttimeColumn] = System.Convert.DBNull;
            }
            
            public bool IsClassNull() {
                return this.IsNull(this.tablePatrols.ClassColumn);
            }
            
            public void SetClassNull() {
                this[this.tablePatrols.ClassColumn] = System.Convert.DBNull;
            }
            
            public bool IsNumberOfCompetitorsNull() {
                return this.IsNull(this.tablePatrols.NumberOfCompetitorsColumn);
            }
            
            public void SetNumberOfCompetitorsNull() {
                this[this.tablePatrols.NumberOfCompetitorsColumn] = System.Convert.DBNull;
            }
            
            public bool IsDisplayNameNull() {
                return this.IsNull(this.tablePatrols.DisplayNameColumn);
            }
            
            public void SetDisplayNameNull() {
                this[this.tablePatrols.DisplayNameColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class PatrolsRowChangeEvent : EventArgs {
            
            private PatrolsRow eventRow;
            
            private DataRowAction eventAction;
            
            public PatrolsRowChangeEvent(PatrolsRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public PatrolsRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
