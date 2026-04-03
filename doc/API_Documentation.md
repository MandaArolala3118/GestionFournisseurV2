# Documentation API - Évaluation Fournisseur

## Table des matières
- [Introduction](#introduction)
- [Authentification](#authentification)
- [Endpoints](#endpoints)
  - [CampagneController](#campagnecontroller)
  - [CategorieController](#categoriecontroller)
  - [EvaluationController](#evaluationcontroller)
  - [FabricantController](#fabricantcontroller)
  - [FournisseurCampagneController](#fournisseurcampagnecontroller)
  - [LoginController](#logincontroller)
  - [NatureController](#naturecontroller)
  - [PonderationController](#ponderationcontroller)
  - [SiteController](#sitecontroller)

## Introduction

Cette documentation décrit tous les endpoints de l'API REST pour l'application d'évaluation de fournisseurs. L'API utilise l'authentification par autorisation Bearer Token.

### Base URL
```
https://localhost:7123/api
```

### En-têtes requis
```
Authorization: Bearer <token>
Content-Type: application/json
```

## Authentification

Tous les endpoints (sauf ceux de LoginController) nécessitent une authentification via un token JWT.

---

## CampagneController

**Route de base:** `/api/campagne`

### GET /api/campagne
Récupère toutes les campagnes.

**Réponse:**
```json
[
  {
    "id": 1,
    "annee": "2024",
    "nomCampagne": "Campagne Annuelle 2024",
    "active": true
  }
]
```

### GET /api/campagne/{id}
Récupère une campagne par son ID.

**Paramètres:**
- `id` (int): ID de la campagne

**Réponse:**
```json
{
  "id": 1,
  "annee": "2024",
  "nomCampagne": "Campagne Annuelle 2024",
  "active": true
}
```

### POST /api/campagne
Crée une nouvelle campagne.

**Corps de la requête:**
```json
{
  "annee": "2024",
  "nomCampagne": "Nouvelle Campagne",
  "active": false
}
```

**Réponse:** 201 Created avec l'objet créé

### PUT /api/campagne/{id}
Met à jour une campagne existante.

**Paramètres:**
- `id` (int): ID de la campagne

**Corps de la requête:**
```json
{
  "annee": "2024",
  "nomCampagne": "Campagne Modifiée",
  "active": true
}
```

**Réponse:** 204 No Content

### DELETE /api/campagne/{id}
Supprime une campagne.

**Paramètres:**
- `id` (int): ID de la campagne

**Réponse:** 204 No Content

---

## CategorieController

**Route de base:** `/api/categorie`

### GET /api/categorie
Récupère toutes les catégories avec leurs pondérations.

**Réponse:**
```json
[
  {
    "id": 1,
    "nomCategorie": "Services",
    "description": "Catégorie des services",
    "ponderationId": 1,
    "ponderation": {
      "id": 1,
      "categorieId": 1,
      "iTransparence": 10,
      "iiFacture": 15,
      "iiiNotoriete": 10,
      "ivPreventionCorruption": 15,
      "vPreventionDroitHomme": 10,
      "viConformiteVsExigence": 10,
      "viiDelaiLivraison": 10,
      "viiiCommunication": 5,
      "ixDisponibiliteDocuments": 5,
      "xConsigneQhse": 5,
      "xiQualitePrix": 15,
      "xiiDureePaiement": 10,
      "xiiiConformiteFiscale": 10,
      "sommePonderations": 130
    }
  }
]
```

### GET /api/categorie/{id}
Récupère une catégorie par son ID avec sa pondération.

**Paramètres:**
- `id` (int): ID de la catégorie

### POST /api/categorie
Crée une nouvelle catégorie.

**Corps de la requête:**
```json
{
  "nomCategorie": "Nouvelle Catégorie",
  "description": "Description de la catégorie"
}
```

### PUT /api/categorie/{id}
Met à jour une catégorie existante.

**Paramètres:**
- `id` (int): ID de la catégorie

**Corps de la requête:**
```json
{
  "nomCategorie": "Catégorie Modifiée",
  "description": "Description modifiée"
}
```

### DELETE /api/categorie/{id}
Supprime une catégorie.

**Paramètres:**
- `id` (int): ID de la catégorie

---

## EvaluationController

**Route de base:** `/api/evaluation`

### GET /api/evaluation/statistiques-campagne-active
Récupère les statistiques de la campagne active.

**Réponse:**
```json
{
  "campagneId": 1,
  "nomCampagne": "Campagne Annuelle 2024",
  "annee": "2024",
  "nombreTotalFournisseurs": 50,
  "nombreFournisseursEvalues": 30,
  "nombreFournisseursNonEvalues": 20,
  "nombreTotalEvaluations": 75,
  "pourcentageEvalues": 60.0
}
```

### GET /api/evaluation/statistiques-campagne/{campagneId}
Récupère les statistiques d'une campagne spécifique.

**Paramètres:**
- `campagneId` (int): ID de la campagne

---

## FabricantController

**Route de base:** `/api/fabricant`

### GET /api/fabricant
Récupère tous les fabricants.

**Réponse:**
```json
[
  {
    "id": 1,
    "nomFabricant": "Fabricant A"
  }
]
```

### GET /api/fabricant/{id}
Récupère un fabricant par son ID.

**Paramètres:**
- `id` (int): ID du fabricant

### POST /api/fabricant
Crée un nouveau fabricant.

**Corps de la requête:**
```json
{
  "nomFabricant": "Nouveau Fabricant"
}
```

### PUT /api/fabricant/{id}
Met à jour un fabricant existant.

**Paramètres:**
- `id` (int): ID du fabricant

**Corps de la requête:**
```json
{
  "nomFabricant": "Fabricant Modifié"
}
```

### DELETE /api/fabricant/{id}
Supprime un fabricant.

**Paramètres:**
- `id` (int): ID du fabricant

---

## FournisseurCampagneController

**Route de base:** `/api/fournisseurcampagne`

### GET /api/fournisseurcampagne/fournisseurs-campagne-active
Récupère tous les fournisseurs de la campagne active.

**Réponse:**
```json
[
  {
    "idFournisseur": 1,
    "vendorNameFournisseur": "Fournisseur A",
    "fabricant": "Fabricant A",
    "categorie": "Services",
    "nature": "Nature A",
    "evaluation": true
  }
]
```

### GET /api/fournisseurcampagne/fournisseur/{id}
Récupère les détails d'un fournisseur par son ID.

**Paramètres:**
- `id` (int): ID du fournisseur

---

## LoginController

**Route de base:** `/api/login`

### GET /api/login/windowsIdentity
Récupère l'identité Windows de l'utilisateur connecté. (Nécessite autorisation)

**Réponse:**
```json
"username"
```

### GET /api/login/getDetailsLogin
Récupère les détails de connexion de l'utilisateur depuis LDAP. (Nécessite autorisation)

**Réponse:**
```json
{
  "username": "utilisateur",
  "email": "utilisateur@domaine.com",
  "fullName": "Nom Complet",
  "department": "Département",
  "title": "Poste"
}
```

---

## NatureController

**Route de base:** `/api/nature`

### GET /api/nature
Récupère toutes les natures.

**Réponse:**
```json
[
  {
    "id": 1,
    "libelNature": "Nature A"
  }
]
```

### GET /api/nature/{id}
Récupère une nature par son ID.

**Paramètres:**
- `id` (int): ID de la nature

### POST /api/nature
Crée une nouvelle nature.

**Corps de la requête:**
```json
{
  "libelNature": "Nouvelle Nature"
}
```

### PUT /api/nature/{id}
Met à jour une nature existante.

**Paramètres:**
- `id` (int): ID de la nature

**Corps de la requête:**
```json
{
  "libelNature": "Nature Modifiée"
}
```

### DELETE /api/nature/{id}
Supprime une nature.

**Paramètres:**
- `id` (int): ID de la nature

---

## PonderationController

**Route de base:** `/api/ponderation`

### GET /api/ponderation
Récupère toutes les pondérations avec leurs catégories.

**Réponse:**
```json
[
  {
    "id": 1,
    "categorieId": 1,
    "iTransparence": 10,
    "iiFacture": 15,
    "iiiNotoriete": 10,
    "ivPreventionCorruption": 15,
    "vPreventionDroitHomme": 10,
    "viConformiteVsExigence": 10,
    "viiDelaiLivraison": 10,
    "viiiCommunication": 5,
    "ixDisponibiliteDocuments": 5,
    "xConsigneQhse": 5,
    "xiQualitePrix": 15,
    "xiiDureePaiement": 10,
    "xiiiConformiteFiscale": 10,
    "sommePonderations": 130,
    "categorie": {
      "id": 1,
      "nomCategorie": "Services",
      "description": "Catégorie des services"
    }
  }
]
```

### GET /api/ponderation/{id}
Récupère une pondération par son ID.

**Paramètres:**
- `id` (int): ID de la pondération

---

## SiteController

**Route de base:** `/api/site`

### GET /api/site
Récupère tous les sites.

**Réponse:**
```json
[
  {
    "id": 1,
    "nomSite": "Site A"
  }
]
```

### GET /api/site/{id}
Récupère un site par son ID.

**Paramètres:**
- `id` (int): ID du site

### POST /api/site
Crée un nouveau site.

**Corps de la requête:**
```json
{
  "nomSite": "Nouveau Site"
}
```

### PUT /api/site/{id}
Met à jour un site existant.

**Paramètres:**
- `id` (int): ID du site

**Corps de la requête:**
```json
{
  "nomSite": "Site Modifié"
}
```

### DELETE /api/site/{id}
Supprime un site.

**Paramètres:**
- `id` (int): ID du site

---

## Codes d'erreur standards

- **200 OK**: Requête réussie
- **201 Created**: Ressource créée avec succès
- **204 No Content**: Requête réussie sans contenu de réponse
- **400 Bad Request**: Requête invalide (validation échouée)
- **401 Unauthorized**: Non authentifié
- **403 Forbidden**: Accès refusé
- **404 Not Found**: Ressource non trouvée
- **500 Internal Server Error**: Erreur serveur interne

---

## Notes importantes

1. Tous les endpoints retournent des réponses au format JSON
2. Les dates sont au format ISO 8601
3. L'authentification est requise pour la plupart des endpoints
4. Les suppressions peuvent échouer si les ressources sont utilisées par d'autres entités
5. Les logs d'erreurs sont enregistrés pour le débogage
