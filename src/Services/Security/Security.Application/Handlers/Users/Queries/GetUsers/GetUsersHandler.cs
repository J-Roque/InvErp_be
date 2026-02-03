using Security.Application.Interfaces.Persistance;
using Security.Domain.Parameters.User;
using FluentValidation;
using System.Text.Json.Nodes;
using Security.Application.Helpers;

namespace Security.Application.Handlers.Users.Queries.GetUsers
{
    public class GetUsersHandler : IQueryHandler<GetUsersQuery, GetUsersResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var page = query.PaginationRequest.Page;
            var pageSize = query.PaginationRequest.PageSize;
            var ignorePagination = query.PaginationRequest.IgnorePagination == 1 ? true : false;

            // Filtros
            var searchTerm = query.PaginationRequest.SearchTerm;
            searchTerm = !string.IsNullOrWhiteSpace(searchTerm) ? searchTerm : null;

            string sort = query.PaginationRequest.Sort.ToLower();
            string order = query.PaginationRequest.Order.Trim().Equals("asc", StringComparison.OrdinalIgnoreCase)
                             ? "asc"
                             : "desc";

            string? startDate = query.PaginationRequest.StartDate;
            startDate = !string.IsNullOrWhiteSpace(startDate) ? startDate : null;

            string? endDate = query.PaginationRequest.EndDate;
            endDate = !string.IsNullOrWhiteSpace(endDate) ? endDate : null;

            // Declarar variables para las fechas parseadas
            DateTime parsedStartDate = default;
            DateTime parsedEndDate = default;

            // Validar fechas
            if (!string.IsNullOrEmpty(startDate) && !DateTime.TryParse(startDate, out parsedStartDate))
            {
                throw new ValidationException("El campo StartDate no es una fecha válida.");
            }

            if (!string.IsNullOrEmpty(endDate) && !DateTime.TryParse(endDate, out parsedEndDate))
            {
                throw new ValidationException("El campo EndDate no es una fecha válida.");
            }

            // Validar que StartDate sea menor que EndDate
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                if (parsedStartDate > parsedEndDate)
                {
                    throw new ValidationException("El campo StartDate no puede ser mayor que EndDate.");
                }
            }

            JsonObject filters = new()
            {
                ["SearchTerm"] = searchTerm,
                ["Sort"] = sort,
                ["Order"] = order,
                ["StartDate"] = string.IsNullOrEmpty(startDate) ? null : parsedStartDate,
                ["EndDate"] = string.IsNullOrEmpty(endDate) ? null : parsedEndDate,
            };

            var filtersJson = filters
                .Where(kv => kv.Value is not null)
                .ToDictionary(kv => kv.Key, kv => kv.Value)
                .ToJsonStringOrNull();

            var paginationParameters = new UserPaginationParameter
            {
                Page = page,
                PageSize = pageSize,
                IgnorePagination = ignorePagination,
                Filters = filtersJson
            };

            var paginatedResult = await _unitOfWork.Users.GetUsersWithFiltersAndPagination(paginationParameters);

            return new GetUsersResult
               (
                    Data: paginatedResult.Data,
                    TotalCount: paginatedResult.TotalCount,
                    Page: paginatedResult.Page,
                    PageSize: paginatedResult.PageSize
               );
        }
    }
}
