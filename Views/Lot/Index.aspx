<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Lot.LotIndexPage" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="AkohoAspx.Models" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Lots - Akoho ASPX</title>
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
        <h1 class="mb-4">Gestion des lots</h1>

        <% if (TempData["LotError"] != null) { %>
            <div class="alert alert-danger" role="alert"><%: TempData["LotError"] %></div>
        <% } %>
        <% if (TempData["LotSuccess"] != null) { %>
            <div class="alert alert-success" role="alert"><%: TempData["LotSuccess"] %></div>
        <% } %>

        <div class="card mb-4">
            <div class="card-body">
                <h2 class="h5">Creer un lot</h2>
                <form action="/Lot/Create" method="post">
                    <%= Html.AntiForgeryToken() %>
                    <div class="row g-3">
                        <div class="col-12 col-md-6">
                            <label for="nomLot" class="form-label">Nom du lot</label>
                            <input id="nomLot" name="nomLot" class="form-control" />
                        </div>
                        <div class="col-12 col-md-6">
                            <label for="raceId" class="form-label">Race</label>
                            <select id="raceId" name="raceId" class="form-select">
                                <option value="">-- Sélectionner une race --</option>
                                <% foreach (var race in RaceOptions) { %>
                                    <option value="<%: race.Id %>">
                                        <%: race.Nom %>
                                    </option>
                                <% } %>
                            </select>
                        </div>
                        <div class="col-12 col-md-4">
                            <label for="nombreInitial" class="form-label">Nombre initial</label>
                            <input id="nombreInitial" name="nombreInitial" type="number" min="0" value="0" class="form-control" />
                        </div>
                        <div class="col-12 col-md-4">
                            <label for="poidsAchat" class="form-label">Poids achat (g)</label>
                            <input id="poidsAchat" name="poidsAchat" type="number" min="0" value="0" class="form-control" />
                        </div>
                        <div class="col-12 col-md-4">
                            <label for="totalInvesti" class="form-label">Total investi</label>
                            <input id="totalInvesti" name="totalInvesti" type="number" min="0" step="0.01" value="0" class="form-control" />
                        </div>
                    </div>
                    <div class="mt-3">
                        <button type="submit" class="btn btn-primary">Creer le lot</button>
                    </div>
                </form>
            </div>
        </div>

        <% if (Model != null && Model.Any()) { %>
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
                        <tbody>
                        <% foreach (var lot in Model) { %>
                            <tr>
                                <td><%: lot.Id %></td>
                                <td><%: lot.NomLot %></td>
                                <td><%: lot.Race != null ? lot.Race.Nom : lot.RaceId.ToString() %></td>
                                <td><%: lot.NombreInitial %></td>
                                <td><%: lot.PoidsAchat %></td>
                                <td><%: lot.TotalInvesti %></td>
                                <td><%: lot.Creation.ToString("yyyy-MM-dd HH:mm") %></td>
                            </tr>
                        <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        <% } %>
    </div>
</body>
</html>

