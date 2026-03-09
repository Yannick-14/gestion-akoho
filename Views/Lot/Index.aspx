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
    <title>Gestion des Lots - Akoho ASPX</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700;800&display=swap" rel="stylesheet">
    <style>
        :root {
            --primary-blue: #0f172a;
            --accent-blue: #3b82f6;
            --glass-bg: rgba(255, 255, 255, 0.7);
            --panel-shadow: 0 20px 40px -15px rgba(0, 0, 0, 0.08);
        }

        body {
            background-color: #f8fafc;
            background-image: radial-gradient(at 0% 0%, rgba(59, 130, 246, 0.05) 0px, transparent 50%),
                              radial-gradient(at 100% 100%, rgba(59, 130, 246, 0.03) 0px, transparent 50%);
            font-family: 'Inter', sans-serif;
            color: #1e293b;
            min-height: 100vh;
        }

        .section-title {
            font-weight: 800;
            letter-spacing: -0.025em;
            color: var(--primary-blue);
        }

        .glass-card {
            background: var(--glass-bg);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            border: 1px solid rgba(255, 255, 255, 0.5);
            border-radius: 24px;
            box-shadow: var(--panel-shadow);
            padding: 2.5rem;
        }

        .form-label-premium {
            font-weight: 700;
            font-size: 0.75rem;
            text-transform: uppercase;
            color: #64748b;
            margin-bottom: 0.5rem;
        }

        .input-premium {
            border: 2px solid #f1f5f9;
            border-radius: 12px;
            padding: 0.75rem 1rem;
            font-weight: 600;
            transition: all 0.2s;
        }

        .input-premium:focus {
            border-color: var(--accent-blue);
            box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.1);
        }

        .btn-create {
            background: linear-gradient(135deg, #1e293b 0%, #0f172a 100%);
            color: white;
            border: none;
            padding: 0.8rem 2rem;
            border-radius: 12px;
            font-weight: 700;
            transition: all 0.3s;
        }

        .btn-create:hover {
            transform: translateY(-2px);
            box-shadow: 0 10px 20px -5px rgba(15, 23, 42, 0.3);
            color: white;
        }

        .table-container {
            background: white;
            border-radius: 20px;
            overflow: hidden;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
            border: 1px solid #f1f5f9;
        }

        .table-premium {
            margin-bottom: 0;
        }

        .table-premium thead th {
            background: #f8fafc;
            border-bottom: 2px solid #f1f5f9;
            color: #64748b;
            font-weight: 700;
            font-size: 0.75rem;
            text-transform: uppercase;
            padding: 1.25rem 1.5rem;
        }

        .table-premium tbody td {
            padding: 1.25rem 1.5rem;
            color: #334155;
            font-weight: 500;
            border-bottom: 1px solid #f1f5f9;
        }

        .table-premium tbody tr:last-child td {
            border-bottom: none;
        }

        .table-premium tbody tr:hover td {
            background-color: #f1f5f9;
        }

        .badge-id {
            background: #f1f5f9;
            color: #475569;
            font-family: monospace;
            font-weight: 700;
        }
    </style>
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>
    
    <div class="container py-5">
        <header class="mb-5">
            <h1 class="section-title h2 mb-1">Gestion des Lots</h1>
            <p class="text-muted fw-500">Administrer et créer les lots de croissance</p>
        </header>

        <% if (TempData["LotError"] != null) { %>
            <div class="alert alert-danger rounded-4 border-0 shadow-sm mb-4" role="alert">
                <strong>Erreur :</strong> <%: TempData["LotError"] %>
            </div>
        <% } %>
        <% if (TempData["LotSuccess"] != null) { %>
            <div class="alert alert-success rounded-4 border-0 shadow-sm mb-4" role="alert">
                <strong>Succès :</strong> <%: TempData["LotSuccess"] %>
            </div>
        <% } %>

        <div class="glass-card mb-5">
            <h2 class="h5 fw-800 mb-4 d-flex align-items-center gap-2">
                Nouveau Lot de Croissance
            </h2>
            <form action="/Lot/Create" method="post">
                <%= Html.AntiForgeryToken() %>
                <div class="row g-4">
                    <div class="col-12 col-md-6">
                        <label for="nomLot" class="form-label-premium">Nom du lot</label>
                        <input id="nomLot" name="nomLot" class="form-control input-premium" placeholder="Ex: Lot Poussins Mars" required />
                    </div>
                    <div class="col-12 col-md-6">
                        <label for="raceId" class="form-label-premium">Race de poulets</label>
                        <select id="raceId" name="raceId" class="form-select input-premium" required>
                            <option value="">Sélectionner une race</option>
                            <% foreach (var race in RaceOptions) { %>
                                <option value="<%: race.Id %>"><%: race.Nom %></option>
                            <% } %>
                        </select>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="nombreInitial" class="form-label-premium">Effectif initial</label>
                        <input id="nombreInitial" name="nombreInitial" type="number" min="1" value="0" class="form-control input-premium" required />
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="poidsAchat" class="form-label-premium">Poids moyen au démarrage (g)</label>
                        <input id="poidsAchat" name="poidsAchat" type="number" min="0" value="0" class="form-control input-premium" required />
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="totalInvesti" class="form-label-premium">Budget total d'achat (Ar)</label>
                        <input id="totalInvesti" name="totalInvesti" type="number" min="0" step="0.01" value="0" class="form-control input-premium" required />
                    </div>
                </div>
                <div class="mt-4 pt-2">
                    <button type="submit" class="btn btn-create">Créer</button>
                </div>
            </form>
        </div>

        <% if (Model != null && Model.Any()) { %>
            <div class="mb-4 d-flex justify-content-between align-items-center">
                <h2 class="h5 fw-800 mb-0">Archives des Lots</h2>
                <span class="badge bg-white text-dark border rounded-pill px-3 py-2 shadow-sm fw-700">
                    Total : <%: Model.Count() %> lots
                </span>
            </div>
            <div class="table-container shadow-sm">
                <div class="table-responsive">
                    <table class="table table-premium align-middle">
                        <thead>
                            <tr>
                                <th># ID</th>
                                <th>Nom du Lot</th>
                                <th>Race</th>
                                <th>Effectif</th>
                                <th>Poids Initial</th>
                                <th>Investissement</th>
                                <th>Date de Création</th>
                                <th class="text-center">Détails</th>
                            </tr>
                        </thead>
                        <tbody>
                        <% foreach (var lot in Model) { %>
                            <tr>
                                <td><span class="badge badge-id rounded-pill px-2 py-1"><%: lot.Id %></span></td>
                                <td class="fw-700 text-primary"><%: lot.NomLot %></td>
                                <td><span class="fw-600"><%: lot.Race != null ? lot.Race.Nom : lot.RaceId.ToString() %></span></td>
                                <td class="fw-bold"><%: lot.NombreInitial %></td>
                                <td><%: lot.PoidsInitiale %> g</td>
                                <td class="text-success fw-bold"><%: lot.PrixAchat.ToString("N2") %> Ar</td>
                                <td class="text-muted small fw-600"><%: lot.Creation.ToString("dd/MM/yyyy HH:mm") %></td>
                                <td class="text-center">
                                    <button class="btn btn-sm btn-outline-primary rounded-circle border-2 p-0 d-inline-flex align-items-center justify-content-center" 
                                            style="width: 32px; height: 32px;"
                                            data-bs-toggle="modal" 
                                            data-bs-target="#modal_<%: lot.Id %>"
                                            title="Plus de détails">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-info-circle-fill" viewBox="0 0 16 16">
                                          <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16m.93-9.412-1 4.705c-.07.34.029.533.308.533.19 0 .452-.083.581-.182l.178-.117-.189-.723c-.338-1.282-.128-1.27.191-1.27.3 0 .524.116.637.21l.178.118-.309-1.393L8.93 6.588zM8 4a1.1 1.1 0 1 1 0-2.2 1.1 1.1 0 0 1 0 2.2"/>
                                        </svg>
                                    </button>

                                    <!-- Modal de Détails -->
                                    <div class="modal fade text-start" id="modal_<%: lot.Id %>" tabindex="-1" aria-hidden="true">
                                        <div class="modal-dialog modal-dialog-centered">
                                            <div class="modal-content border-0 shadow-lg rounded-4 overflow-hidden">
                                                <div class="modal-header bg-primary text-white border-0 py-3">
                                                    <h5 class="modal-title fw-bold">Détails du Lot #<%: lot.Id %></h5>
                                                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                                </div>
                                                <div class="modal-body p-4">
                                                    <div class="row g-3">
                                                        <div class="col-6">
                                                            <div class="p-3 bg-light rounded-3">
                                                                <small class="text-muted d-block text-uppercase fw-700" style="font-size: 0.65rem;">Nom du Lot</small>
                                                                <span class="fw-bold"><%: lot.NomLot %></span>
                                                            </div>
                                                        </div>
                                                        <div class="col-6">
                                                            <div class="p-3 bg-light rounded-3">
                                                                <small class="text-muted d-block text-uppercase fw-700" style="font-size: 0.65rem;">Race</small>
                                                                <span class="fw-bold"><%: lot.Race != null ? lot.Race.Nom : lot.RaceId.ToString() %></span>
                                                            </div>
                                                        </div>
                                                        <div class="col-6">
                                                            <div class="p-3 bg-light rounded-3">
                                                                <small class="text-muted d-block text-uppercase fw-700" style="font-size: 0.65rem;">Effectif Initial</small>
                                                                <span class="fw-bold"><%: lot.NombreInitial %></span>
                                                            </div>
                                                        </div>
                                                        <div class="col-6">
                                                            <div class="p-3 bg-light rounded-3">
                                                                <small class="text-muted d-block text-uppercase fw-700" style="font-size: 0.65rem;">Poids Initial</small>
                                                                <span class="fw-bold"><%: lot.PoidsInitiale %> g</span>
                                                            </div>
                                                        </div>
                                                        <div class="col-12">
                                                            <div class="p-3 border rounded-3 bg-white">
                                                                <small class="text-muted d-block text-uppercase fw-700 mb-1" style="font-size: 0.65rem;">Investissement Total</small>
                                                                <span class="h5 fw-800 text-success mb-0"><%: lot.PrixAchat.ToString("N2") %> Ar</span>
                                                            </div>
                                                        </div>
                                                        <div class="col-12">
                                                            <div class="p-2 text-center text-muted small fw-600 border-top mt-2 pt-3">
                                                                Créé le : <%: lot.Creation.ToString("dd MMMM yyyy à HH:mm") %>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        <% } %>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>


