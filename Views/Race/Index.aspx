<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Race.RaceIndexPage" %>
<%@ Import Namespace="System.Linq" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Races - Akoho ASPX</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>

    <div class="container">
        <h1 class="mb-4">Gestion des races</h1>

        <% if (TempData["RaceError"] != null) { %>
            <div class="alert alert-danger" role="alert"><%: TempData["RaceError"] %></div>
        <% } %>
        <% if (TempData["RaceSuccess"] != null) { %>
            <div class="alert alert-success" role="alert"><%: TempData["RaceSuccess"] %></div>
        <% } %>

        <% if (!HasCurrentRace) { %>
            <div class="card mb-4">
                <div class="card-body">
                    <h2 class="h5">Ajouter une race</h2>
                    <form action="/Race/Create" method="post">
                        <%= Html.AntiForgeryToken() %>
                        <div class="mb-3">
                            <label for="nom" class="form-label">Nom</label>
                            <input id="nom" name="nom" class="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="jourFoyAtody" class="form-label">Jour d' eclosion</label>
                            <input id="jourFoyAtody" name="jourFoyAtody" type="number" min="1" class="form-control" />
                        </div>
                        <button type="submit" class="btn btn-primary">Creer</button>
                    </form>
                </div>
            </div>
        <% } else { %>
            <div class="alert alert-info d-flex justify-content-between align-items-center">
                <div>Race active (session): <strong><%: CurrentRaceId %></strong></div>
                <div>
                    <% using (Html.BeginForm("ResetCurrentRace", "Race", FormMethod.Post)) { %>
                        <%= Html.AntiForgeryToken() %>
                        <button type="submit" class="btn btn-sm btn-outline-secondary">Changer de race</button>
                    <% } %>
                </div>
            </div>

            <div class="card shadow-sm border-0 mb-4">
                <div class="card-body py-3 px-3">
                    <h2 class="h6 mb-3">Ajouter les croissances</h2>
                    <p class="text-muted small mb-3">Soumettez plusieurs fois le formulaire pour ajouter plusieurs lignes.</p>

                    <% using (Html.BeginForm("createCroissanceRace", "Race", FormMethod.Post, new { @class = "small" })) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="hidden" name="raceId" value="<%: CurrentRaceId %>" />

                        <div class="row g-2">
                            <div class="col-12 col-lg-6">
                                <div class="d-flex justify-content-between align-items-center mb-2">
                                    <h3 class="h6 mb-0">CroissancePoidsRace</h3>
                                    <button id="addLeftItem" type="button" class="btn btn-sm btn-outline-secondary">Ajouter ligne</button>
                                </div>
                                <div id="leftItemsContainer" class="vstack gap-2">
                                    <div class="race-item border rounded p-2">
                                        <div class="text-muted small mb-2">Gauche #1</div>
                                        <div class="mb-2">
                                            <label class="form-label mb-1" for="left-semaine-0">Semaine</label>
                                            <input id="left-semaine-0" name="leftItems[0].Semaine" class="form-control form-control-sm" />
                                        </div>
                                        <div>
                                            <label class="form-label mb-1" for="left-value-0">Poids (g)</label>
                                            <input id="left-value-0" name="leftItems[0].Poids" type="number" min="0" class="form-control form-control-sm" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-12 col-lg-6">
                                <div class="d-flex justify-content-between align-items-center mb-2">
                                    <h3 class="h6 mb-0">CroissanceAlimentRace</h3>
                                    <button id="addRightItem" type="button" class="btn btn-sm btn-outline-secondary">Ajouter ligne</button>
                                </div>
                                <div id="rightItemsContainer" class="vstack gap-2">
                                    <div class="race-item border rounded p-2">
                                        <div class="text-muted small mb-2">Droite #1</div>
                                        <div class="mb-2">
                                            <label class="form-label mb-1" for="right-semaine-0">Semaine</label>
                                            <input id="right-semaine-0" name="rightItems[0].Semaine" class="form-control form-control-sm" />
                                        </div>
                                        <div>
                                            <label class="form-label mb-1" for="right-value-0">Aliment (g)</label>
                                            <input id="right-value-0" name="rightItems[0].Aliment" type="number" min="0" class="form-control form-control-sm" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="text-center mt-3">
                            <button type="submit" class="btn btn-primary btn-sm">Inserer</button>
                        </div>
                    <% } %>
                </div>
            </div>

            <div class="card mb-4">
                <div class="card-body">
                    <h2 class="h5">Ajouter un prix unitaire de vente</h2>
                    <% using (Html.BeginForm("addPrixUnitaire", "Race", FormMethod.Post)) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="hidden" name="raceId" value="<%: CurrentRaceId %>" />
                        <div class="mb-3">
                            <label for="prix" class="form-label">Prix par gramme</label>
                            <input id="prix" name="prix" type="number" min="1" class="form-control" />
                        </div>
                        <button type="submit" class="btn btn-primary">Valider</button>
                    <% } %>
                </div>
            </div>
        <% } %>

        <% if (Model != null && Model.Any()) { %>
            <div class="card mt-4">
                <div class="card-body">
                    <h2 class="h6">Races existantes</h2>
                    <table class="table table-sm">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Nom</th>
                                <th>JourFoyAtody</th>
                            </tr>
                        </thead>
                        <tbody>
                        <% foreach (var race in Model) { %>
                            <tr>
                                <td><%: race.Id %></td>
                                <td><%: race.Nom %></td>
                                <td><%: race.JourFoyAtody %></td>
                            </tr>
                        <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        <% } %>
    </div>

    <script>
        (function () {
            function buildItemHtml(side, index) {
                var sideTitle = side === 'left' ? 'Gauche' : 'Droite';
                var prefix = side === 'left' ? 'leftItems' : 'rightItems';
                var valueLabel = side === 'left' ? 'Poids (g)' : 'Aliment (g)';
                var valueProp = side === 'left' ? 'Poids' : 'Aliment';
                var semaineId = side + '-semaine-' + index;
                var valueId = side + '-value-' + index;

                return '<div class="race-item border rounded p-2">'
                    + '<div class="text-muted small mb-2">' + sideTitle + ' #' + (index + 1) + '</div>'
                    + '<div class="mb-2">'
                    + '<label class="form-label mb-1" for="' + semaineId + '">Semaine</label>'
                    + '<input id="' + semaineId + '" name="' + prefix + '[' + index + '].Semaine" class="form-control form-control-sm" />'
                    + '</div>'
                    + '<div>'
                    + '<label class="form-label mb-1" for="' + valueId + '">' + valueLabel + '</label>'
                    + '<input id="' + valueId + '" name="' + prefix + '[' + index + '].' + valueProp + '" type="number" min="0" class="form-control form-control-sm" />'
                    + '</div>'
                    + '</div>';
            }

            function addItem(side) {
                var container = document.getElementById(side + 'ItemsContainer');
                if (!container) {
                    return;
                }

                var index = container.querySelectorAll('.race-item').length;
                container.insertAdjacentHTML('beforeend', buildItemHtml(side, index));
            }

            var addLeftBtn = document.getElementById('addLeftItem');
            if (addLeftBtn) {
                addLeftBtn.addEventListener('click', function () {
                    addItem('left');
                });
            }

            var addRightBtn = document.getElementById('addRightItem');
            if (addRightBtn) {
                addRightBtn.addEventListener('click', function () {
                    addItem('right');
                });
            }
        })();
    </script>
</body>
</html>

