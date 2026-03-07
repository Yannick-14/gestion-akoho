<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Home.HomeIndexPage" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Accueil - Akoho ASPX</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>

    <div class="container">
        <div class="text-center">
            <h1 class="display-5">Bienvenue</h1>
            <p>Application de gestion des lots d'akoho.</p>
        </div>
    </div>
</body>
</html>
