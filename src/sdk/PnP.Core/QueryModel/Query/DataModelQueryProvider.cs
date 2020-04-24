﻿using PnP.Core.Model;
using PnP.Core.QueryModel.Linq;
using PnP.Core.QueryModel.Model;
using PnP.Core.QueryModel.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PnP.Core.QueryModel.Query
{
    /// <summary>
    /// Concrete implementation of an IQueryProvider for the Domain Model
    /// </summary>
    /// <typeparam name="TModel">The Type of the Domain Model object that the IQueryProvider supports</typeparam>
    internal class DataModelQueryProvider<TModel> : BaseQueryProvider
    {
        /// <summary>
        /// The internal Query Service used to execute the actual queries
        /// </summary>
        private DataModelQueryService<TModel> queryService;

        #region Constructors

        // Keep this constructor protected to avoid creation from outside
        protected DataModelQueryProvider() { }

        /// <summary>
        /// Creates a new instance of the IQueryProvider based on an external Query Service
        /// </summary>
        /// <param name="queryService"></param>
        public DataModelQueryProvider(DataModelQueryService<TModel> queryService)
        {
            this.queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
        }

        #endregion

        #region BaseQueryProvider abstract methods implementation

        public override object Execute(Expression expression)
        {
            // Translate the query expression into an actual query text for the target Query Service
            var query = this.Translate(expression);

            // Execute the query via the target Query Service
            return queryService.ExecuteQuery(query);
        }

        public override IQueryable CreateQuery(Expression expression)
        {
            var result = new QueryableDataModelCollection<TModel>(this, expression);
            return result;
        }

        #endregion

        /// <summary>
        /// Internal method to execute the translation of the query expression 
        /// into an actual query text for the target Query Service
        /// </summary>
        /// <param name="expression">The expression to translate</param>
        /// <returns>The expression translated into the actual query text for the target Query Service</returns>
        internal ODataQuery<TModel> Translate(Expression expression)
        {
            return new DataModelQueryTranslator<TModel>().Translate(expression);
        }
    }

}
