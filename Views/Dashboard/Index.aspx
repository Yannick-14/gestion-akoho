<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Dashboard.DashboardIndexPage" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Dashboard - Elevage Akoho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body>
<% Html.RenderPartial("Navigation"); %>
<div class="card">
    <div class="card-body">
        <h2 class="h6">Lots existants</h2>
        <table class="table table-sm table-striped align-middle">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Nom</th>
                    <th>Race</th>
                    <th>Nombre</th>
                    <th>Poids</th>
                    <th>Total investi</th>
                    <th>Creation</th>
                </tr>
            </thead>
        </table>
    </div>
</div>
</body>