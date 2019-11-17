using Pchp.Core;
using Pchp.Core.QueryValue;
using Pchp.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary1;
using LiteDB;

public class MyDb //: wpdb
{
    public PhpValue error;

    public PhpValue _suppress_errors = false;

    public PhpValue wp_prefix = "";

    public PhpValue prefix = "";

    public PhpValue base_prefix;

    public PhpValue blogid = 0;

    public PhpValue siteid = 0;

    private PhpValue table_names = PhpValue.FromClr((object)new string[] { "posts",
        "comments",
        "links",
        "options",
        "postmeta",
        "terms",
        "term_taxonomy",
        "term_relationships",
        "termmeta",
        "commentmeta"});

    public PhpValue global_tables = PhpValue.FromClr((object)new string[] { "users", "usermeta" });

    public PhpValue ms_global_tables = PhpValue.FromClr((object)new string[] { "blogs",
        "blogmeta",
        "signups",
        "site",
        "sitemeta",
        "sitecategories",
        "registration_log"});

    public PhpValue comments;
    public PhpValue commentmeta;
    public PhpValue links;
    public PhpValue options = "options";//not sure where this is set
    public PhpValue postmeta;
    public PhpValue posts;
    public PhpValue terms;
    public PhpValue term_relationships;
    public PhpValue term_taxonomy;
    public PhpValue termmeta;
    public PhpValue usermeta;
    public PhpValue users;
    public PhpValue blogs;
    public PhpValue blogmeta;
    public PhpValue registration_log;
    public PhpValue signups;
    public PhpValue site;
    public PhpValue sitecategories;
    public PhpValue sitemeta;
    public PhpValue field_types = PhpValue.FromClr((object)new string[0]);
    public PhpValue charset;
    public PhpValue collate;
    LiteRepository repo;

    public MyDb()
    {
        //ContextExtensions.CurrentContext.DeclareFunction()
        string connectionString = $"filename=wp.db;";
        repo = new LiteRepository(connectionString);
        repo.Database.GetCollection<Options>().EnsureIndex(o => o.option_name);
    }

    //public MyDb(Context ctx, QueryValue<DummyFieldsOnlyCtor> _) : base(ctx, _)
    //{
    //}

    //public MyDb(Context ctx, PhpValue dbuser, PhpValue dbpassword, PhpValue dbname, PhpValue dbhost) : base(ctx, dbuser, dbpassword, dbname, dbhost)
    //{
    //}

    //public MyDb(PhpValue dbuser, PhpValue dbpassword, PhpValue dbname, PhpValue dbhost) : base(dbuser, dbpassword, dbname, dbhost)
    //{
    //}

    public /*override*/ PhpValue db_connect(PhpValue allow_bail = default/*true*/)//sig doesnt match exactly
    {
        return true;
    }

    public /*override*/ PhpValue check_connection(PhpValue allow_bail = default/*true*/)
    {
        return true;
    }

    public /*override*/ PhpValue check_database_version()
    {
        return default;// base.check_database_version();
    }

    public /*override*/ PhpValue close()
    {
        return default;// base.close();
    }

    public /*override*/ PhpValue db_version()
    {
        return default;// base.db_version();
    }

    public /*override*/ PhpValue delete(PhpValue table, PhpValue where, PhpValue where_format = default)
    {
        return default;// base.delete(table, where, where_format);
    }

    public /*override*/ PhpValue get_blog_prefix(PhpValue blog_id = default)
    {
        return "wp_";
        //return base.get_blog_prefix(blog_id);
    }

    public /*override*/ PhpValue insert(PhpValue table, PhpValue data, PhpValue format = default)
    {
        return default;// base.insert(table, data, format);
    }

    public /*override*/ PhpValue query(PhpValue query)
    {
        return default;// base.query(query);
    }

    public /*override*/ PhpValue get_results(PhpValue query = default, [DefaultValue("<get_results.output>_DefaultValue")] PhpValue output = default)
    {
        string q = query.ToString().Trim(';');
        if (q.StartsWith("DESCRIBE"))
        {
            string[] parts = q.Split(' ');
            repo.Database.GetCollection(parts[1]);
            switch (parts[1])
            {

            }
        }
        return false;// base.get_results(query, output);
    }

    public /*override*/ PhpValue get_var(PhpValue query = default, int x = 0, int y = 0)
    {
        string stringVal = query.String.ToString();//um...
        if (!stringVal.StartsWith("SELECT"))
            return default;
        string[] parts = stringVal.Split(new string[] { "SELECT", "FROM", "WHERE" }, StringSplitOptions.RemoveEmptyEntries);

        string option_name = parts[2].Split('\'')[1];
        Options opt = repo.Database.GetCollection<Options>().Find(o => o.option_name == option_name).Skip(x).FirstOrDefault();

        if (opt == default)
            return PhpValue.Void;

        //return base.get_var(query, x, y);
        return opt.option_value;
    }

    public /*override*/ PhpValue set_prefix(PhpValue prefix, bool set_table_names = true/*true*/)
    {
        base_prefix = prefix;
        if (set_table_names)
        {
            table_names = PhpValue.FromClr((object)table_names.GetEnumerator().ToEnumerable().Select(kvp => prefix.ToString() + kvp.Value.String).ToArray());
            global_tables = PhpValue.FromClr((object)global_tables.GetEnumerator().ToEnumerable().Select(kvp => prefix.ToString() + kvp.Value.String).ToArray());
            //old_tables = PhpValue.FromClr((object)tables.GetEnumerator().ToEnumerable().Select(kvp => prefix.String + kvp.Value.String).ToArray());
        }
        return "";
    }

    public PhpValue suppress_errors(bool suppress = true)
    {
        PhpValue errors = _suppress_errors;
        _suppress_errors = suppress;
        return errors;
    }

    public PhpValue tables(string scope = "all", bool prefix = true, int blog_id = 0)
    {
        switch (scope)
        {
            case "all":
                return PhpValue.FromClr((object)global_tables.GetEnumerator().ToEnumerable().Concat(table_names.GetEnumerator().ToEnumerable()).Select(kvp => kvp.Value.ToString()).ToArray());
                break;
            case "blog":
                break;
            case "global":
                break;
            default:
                break;
        }
        return false;
    }

    public PhpValue prepare(string query, params PhpValue[] args)
    {
        return false;
    }

    public PhpValue get_row(string query = default, PhpValue output = default, int y = 0)
    {
        return false;
    }

    //public virtual PhpValue set_prefix(PhpValue prefix)
    //{
    //    return default;
    //}

    //public virtual PhpValue set_prefix()
    //{
    //    return default;
    //}
}
//class stuff : wpdb
//{
//    public override PhpValue prepare(PhpValue query, params PhpValue[] args)
//    {
//        return base.prepare(query, args);
//    }

//    public override PhpValue get_row(PhpValue query = default, [DefaultValue("<get_row.output>_DefaultValue")] PhpValue output = default, PhpValue y = 0L)
//    {
//        return base.get_row(query, output, y);
//    }
//}