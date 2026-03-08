<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Eclosion - Elevage Akoho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body class="bg-light">
    <% Html.RenderPartial("Navigation"); %>

    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-md-8 col-lg-6">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                        <h1 class="h4 mb-0">Eclore les oeufs</h1>
                        <a href="/Dashboard/Index" class="btn btn-sm btn-light text-primary fw-bold">&larr; Retour</a>
                    </div>
                    <div class="card-body p-4">
                        <div class="alert alert-info mb-4">
                            <strong>Details du Lot :</strong><br/>
                            ID Lot : <%: ViewBag.LotOeufId %><br/>
                        </div>

                        <form action="/Lot/CreateNewLot" method="post">
                            <%: Html.AntiForgeryToken() %>
                            <input type="hidden" name="lotOeufId" value="<%: ViewBag.LotOeufId %>" />

                            <div class="mb-3 col-12 col-md-6">
                                <label for="pourcentage" class="form-label">Pourcentage d'eclos</label>
                                <input type="number" class="form-control" id="pourcentage" name="pourcentage" min="1" />
                            </div>

                            <div class="col-12 col-md-6">
                                <label for="nomLot" class="form-label">Nom du lot</label>
                                <input id="nomLot" name="nomLot" class="form-control" />
                            </div>

                            <div class="d-grid mt-4">
                                <button type="submit" class="btn btn-success btn-lg">Creer un nouveau lot</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
