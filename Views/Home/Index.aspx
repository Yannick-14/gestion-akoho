<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Accueil - Akoho ASPX</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body>
    <nav class="navbar navbar-expand-sm navbar-light bg-white border-bottom mb-3">
        <div class="container-fluid">
            <a class="navbar-brand" href="<%: Url.Action("Index", "Home") %>">Akoho ASPX</a>
            <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                <ul class="navbar-nav flex-grow-1">
                    <li class="nav-item"><a class="nav-link text-dark" href="<%: Url.Action("Index", "Home") %>">Home</a></li>
                    <li class="nav-item"><a class="nav-link text-dark" href="<%: Url.Action("Index", "Race") %>">Race</a></li>
                    <li class="nav-item"><a class="nav-link text-dark" href="<%: Url.Action("Index", "Lot") %>">Lot</a></li>
                </ul>
            </div>
        </div>
    </nav>

    <div class="container">
        <div class="text-center">
            <h1 class="display-5">Bienvenue</h1>
            <p>Application de gestion des lots d'akoho.</p>
        </div>
    </div>
</body>
</html>
