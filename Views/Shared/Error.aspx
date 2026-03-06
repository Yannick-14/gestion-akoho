<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<AkohoAspx.Models.ErrorViewModel>" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Error - Akoho ASPX</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body>
    <div class="container mt-4">
        <h1 class="text-danger">Error.</h1>
        <h2 class="text-danger">An error occurred while processing your request.</h2>

        <% if (Model != null && Model.ShowRequestId) { %>
            <p>
                <strong>Request ID:</strong> <code><%: Model.RequestId %></code>
            </p>
        <% } %>
    </div>
</body>
</html>
