# MediaTekDocuments

Cette application permet de gérer les documents (livres, DVD, revues) d'une médiathèque. Elle a été codée en C# sous Visual Studio 2019. C’est une application de bureau, prévue pour être installée sur plusieurs postes accédant à la même base de données.

L’application exploite une API REST pour accéder à la base de données MySQL.

---

## Présentation de l’application d’origine

Toutes les fonctionnalités de base de l’application sont décrites dans le dépôt d’origine disponible ici :  
https://github.com/CNED-SLAM/MediaTekDocuments.git

Ce README présente uniquement les fonctionnalités ajoutées, ainsi que le mode opératoire pour installer et utiliser l’application localement.

---

## Fonctionnalités ajoutées

### Authentification des utilisateurs

- Fenêtre de connexion au démarrage de l’application (nom d’utilisateur + mot de passe).
- Droits d’accès variables selon le service :
  - Services administrateur et administratif : accès complet.
  - Service prêts : accès limité aux onglets de consultation.
  - Service culture : aucun droit → fermeture automatique de l’application.

![image](https://github.com/user-attachments/assets/dbc10fad-50b5-4fa1-968f-341db31977d7)
Plusieurs utilisateurs avec des droits différents ont été configurés :
![image](https://github.com/user-attachments/assets/30a22513-abc7-41f2-9928-bb21289f512a)


---

### Alerte abonnements expirants

- À l'ouverture, une fenêtre affiche les abonnements se terminant dans moins de 30 jours (réservée aux services administrateur et administratif).

![image](https://github.com/user-attachments/assets/9c50c013-0021-43da-9dc2-bc612045bf40)


---

### Gestion des commandes de livres

- Recherche de livre par numéro.
- Affichage :
  - Informations du livre.
  - Liste des commandes associées (triée par date).
  - Détails de la commande sélectionnée.
- Ajout, modification et suppression de commandes.
- Modification de l’étape de suivi avec des règles strictes.

![image](https://github.com/user-attachments/assets/29bc5415-8727-40b4-bc00-fca433481d70)


---

### Gestion des commandes de DVD

- Fonctionnalité identique à celle des livres, adaptée aux DVD.
![image](https://github.com/user-attachments/assets/89239aab-aa69-409b-9f31-f01cb9aaca40)


---

### Gestion des abonnements de revues

- Recherche de revue par numéro.
- Affichage :
  - Informations de la revue.
  - Liste des abonnements.
  - Détails de l’abonnement sélectionné.
- Ajout d’abonnement avec génération automatique des exemplaires associés.
- Suppression possible uniquement si aucun exemplaire n’est rattaché.

![image](https://github.com/user-attachments/assets/3e850235-8466-48da-b256-086cad53c1e6)


---

## Modifications de la base de données

- Ajout de la table `suivi` (étapes de commande).
- Ajout des tables `utilisateur` et `service` pour gérer l’authentification.
- Relations entre les tables pour assurer cohérence et restrictions.

![image](https://github.com/user-attachments/assets/9b907ec4-cc63-43b1-bbe3-a171272206c1)


---

## API REST

- L’application utilise une API REST sécurisée avec authentification basique.
- Le code de l’API est disponible ici :  
https://github.com/cnedienne/rest_mediatekdocuments.git
---

## Installation de l’application

1. Télécharger le fichier `Mediatek.msi` depuis le dépôt.
2. Lancer l’installation en double-cliquant sur le fichier.
3. Une fois installée, l’application est prête à être utilisée.

