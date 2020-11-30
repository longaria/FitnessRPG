-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 03, 2019 at 03:20 AM
-- Server version: 10.1.38-MariaDB
-- PHP Version: 7.3.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `unityaccess`
--

-- --------------------------------------------------------

--
-- Table structure for table `enemies`
--

CREATE TABLE `enemies` (
  `id` varchar(200) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` varchar(300) NOT NULL,
  `xp` int(10) NOT NULL,
  `mingold` int(10) NOT NULL,
  `maxgold` int(10) NOT NULL,
  `stage` int(10) NOT NULL,
  `loot` varchar(500) NOT NULL,
  `stats` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `enemies`
--

INSERT INTO `enemies` (`id`, `name`, `description`, `xp`, `mingold`, `maxgold`, `stage`, `loot`, `stats`) VALUES
('Boss/AlienGreen', 'Green Alien', 'schwäche:,magie', 250, 100, 1000, 10, 'Sprites/Item Icons/Ingredients/7,1,0.4,Sprites/Item Icons/Ingredients/8,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,90,MaxDmg,270,PhysicalDefense,10,MinMDmg,80,MaxMDmg,240,MagicDefense,20,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,1900,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Cat', 'kitty', 'Stärken:,physischer schaden,schwächen:,magischer schaden', 25, 10, 50, 1, 'Sprites/Item Icons/Ingredients/1,1,0.8', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,4,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,100,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Dino', 'Dino', 'stärke:,magie', 225, 90, 450, 9, 'Sprites/Item Icons/Ingredients/5,1,0.4,Sprites/Item Icons/Ingredients/6,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,70,MaxDmg,210,PhysicalDefense,20,MinMDmg,80,MaxMDmg,240,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,1550,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Dog', 'wuffie', 'stärken:,magischer schaden,schwächen:,phyischer schaden', 50, 20, 100, 2, 'Sprites/Item Icons/Ingredients/2,1,0.4,Sprites/Item Icons/Ingredients/7,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,20,MaxMDmg,60,MagicDefense,15,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/FlyingEye1', 'Auge des Horrors', 'viel glück!', 350, 140, 1200, -1, 'Sprites/Item Icons/Ingredients/gem,1,0.4,Sprites/Item Icons/Ingredients/gemGreen,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,30,MaxDmg,90,PhysicalDefense,20,MinMDmg,30,MaxMDmg,90,MagicDefense,20,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,500,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/FlyingEye2', 'Auge des Terrors', 'viel glück!', 350, 140, 1200, -1, 'Sprites/Item Icons/Ingredients/gem,1,0.4,Sprites/Item Icons/Ingredients/gemRed,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,30,MaxDmg,90,PhysicalDefense,20,MinMDmg,30,MaxMDmg,90,MagicDefense,20,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,500,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Orc1', 'Gary', 'stärken:,physischer schaden, schwächen:,magie', 75, 30, 150, 3, 'Sprites/Item Icons/Ingredients/3,1,0.4,Sprites/Item Icons/Ingredients/6,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,30,MaxDmg,90,PhysicalDefense,20,MinMDmg,30,MaxMDmg,30,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,350,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Orc2', 'Harry', 'stärken:,magischer schaden, schwächen:,physischer schaden', 100, 40, 200, 4, 'Sprites/Item Icons/Ingredients/4,1,0.4,Sprites/Item Icons/Ingredients/5,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,30,MaxDmg,90,PhysicalDefense,10,MinMDmg,40,MaxMDmg,120,MagicDefense,20,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,500,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Orc3', 'Berry', 'stärken:,physischer schaden, schwächen:,magie', 125, 50, 250, 5, 'Sprites/Item Icons/Ingredients/8,1,0.4,Sprites/Item Icons/Ingredients/9,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,50,MaxDmg,150,PhysicalDefense,20,MinMDmg,40,MaxMDmg,120,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,650,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/PredatorAlien', 'Predator', 'viel glück!', 350, 140, 1200, 14, 'Sprites/Item Icons/Ingredients/3,1,0.4,Sprites/Item Icons/Ingredients/4,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,130,MaxDmg,390,PhysicalDefense,20,MinMDmg,120,MaxMDmg,360,MagicDefense,20,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,3000,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Pumpkin', 'Pumper', 'stärken:,hohe ausweichchance', 200, 80, 400, 8, 'Sprites/Item Icons/Ingredients/4,1,0.4,Sprites/Item Icons/Ingredients/3,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,70,MaxDmg,210,PhysicalDefense,10,MinMDmg,70,MaxMDmg,210,MagicDefense,20,AttackSpeed,3,CritChance,5,DodgeChance,25,Recovery,5,Endurance,100,Health,1250,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Troll1', 'Grok', 'schwäche:, magie', 275, 110, 1050, 11, 'Sprites/Item Icons/Ingredients/10,1,0.4,Sprites/Item Icons/Ingredients/9,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,90,MaxDmg,270,PhysicalDefense,20,MinMDmg,100,MaxMDmg,300,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,2200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Troll2', 'Mok', 'schwäche:,physischer schaden', 300, 120, 1100, 12, 'Sprites/Item Icons/Ingredients/12,1,0.4,Sprites/Item Icons/Ingredients/11,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,110,MaxDmg,330,PhysicalDefense,10,MinMDmg,100,MaxMDmg,300,MagicDefense,20,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,2400,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/Troll3', 'Rok', 'schwäche:,magie', 325, 130, 1150, 13, 'Sprites/Item Icons/Ingredients/4,1,0.4,Sprites/Item Icons/Ingredients/5,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,110,MaxDmg,330,PhysicalDefense,20,MinMDmg,120,MaxMDmg,360,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,2600,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/ZombieFemale', 'Sally', 'stärken:,magie', 150, 60, 300, 6, 'Sprites/Item Icons/Ingredients/10,1,0.4,Sprites/Item Icons/Ingredients/11,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,50,MaxDmg,50,PhysicalDefense,10,MinMDmg,60,MaxMDmg,180,MagicDefense,20,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,800,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1'),
('Boss/ZombieMale', 'Henry', 'stärken;, physischer schaden', 175, 70, 350, 7, 'Sprites/Item Icons/Ingredients/1,1,0.4,Sprites/Item Icons/Ingredients/2,1,0.4', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,70,MaxDmg,210,PhysicalDefense,20,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,1000,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1');

-- --------------------------------------------------------

--
-- Table structure for table `items`
--

CREATE TABLE `items` (
  `id` varchar(200) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` varchar(100) NOT NULL,
  `type` varchar(100) NOT NULL,
  `cost` int(10) NOT NULL,
  `stats` varchar(300) NOT NULL DEFAULT 'smth',
  `Duration` float NOT NULL,
  `Charges` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `items`
--

INSERT INTO `items` (`id`, `name`, `description`, `type`, `cost`, `stats`, `Duration`, `Charges`) VALUES
('Sprites/Item Icons/Ingredients/1', 'Magische Meeresmuschel', 'erhöht Magische Verteidigung +20 für 10 Sek.', 'Usable', 200, 'MagicDefense,20', 10, 3),
('Sprites/Item Icons/Ingredients/10', 'Splitterbombe', 'zerstört Physische Verteidigung -50 für 10 sek.', 'Weapon', 300, 'PhysicalDefense,-50', 10, 3),
('Sprites/Item Icons/Ingredients/11', 'Ausdauertrank', 'erfrischt 100 ausdauer', 'Usable', 100, 'endurance, 100', 0, 3),
('Sprites/Item Icons/Ingredients/12', 'feuerrubin', 'konvertiert attacken zu feuerschaden', 'Weapon', 500, 'fire, 1', 0, 3),
('Sprites/Item Icons/Ingredients/13', 'frisches brötchen', 'heilt 500 Lebenspunkte', 'Usable', 800, 'health, 500', 0, 3),
('Sprites/Item Icons/Ingredients/14', 'Glas frischer Knochen', 'lenkt hunde ab', 'Weapon', 1000, 'dog, 1', 0, 3),
('Sprites/Item Icons/Ingredients/15', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Ingredients/16', 'Auge des Jägers', 'verringert Gegnerausweichchance - 10% für 10 sek.', 'Weapon', 2000, 'DodgeChance, -10', 10, 3),
('Sprites/Item Icons/Ingredients/17', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Ingredients/18', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Ingredients/19', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Ingredients/2', 'geringer heiltrank', 'heilt 200 Lebenspunkte', 'Usable', 200, 'Health, 200', 0, 3),
('Sprites/Item Icons/Ingredients/20', 'königsmuschel', 'erhöht angriffsgeschwindigkeit um 10% für 10 sek.', 'Usable', 3000, 'AttackSpeed, 10', 10, 3),
('Sprites/Item Icons/Ingredients/3', 'regenerierender ausdauertrank', 'erhöht erholung um 10 für 30 sek.', 'Usable', 500, 'Recovery, 10', 30, 3),
('Sprites/Item Icons/Ingredients/4', 'magisches Erz', 'erhöhte physische Verteidigung um 10 für 10 sek.', 'Usable', 100, 'PhysicalDefense, 10', 10, 3),
('Sprites/Item Icons/Ingredients/5', 'magisches silber', 'erhöht magische verteidigung um 10 für 10 sek.', 'Usable', 100, 'MagicDefense, 10', 10, 3),
('Sprites/Item Icons/Ingredients/6', 'geräucherter schinken', 'heilt 1000 lebenspunkte', 'Usable', 1000, 'Health, 1000', 0, 3),
('Sprites/Item Icons/Ingredients/7', 'magischer Pilz', 'erhöht magische Verteidigung um 50 für 10 sek.', 'Usable', 1200, 'MagicDefense, 50', 10, 3),
('Sprites/Item Icons/Ingredients/8', 'Necronomicon', 'erhöht magischen schaden um 100 für 10 sek.', 'Usable', 1000, 'MinMDmg, 100, MaxMDmg, 100', 10, 3),
('Sprites/Item Icons/Ingredients/9', 'epische karotte', 'erhöht ausweichchance um 50% für 10 sek.', 'Usable', 5000, 'DodgeChance, 50', 10, 3),
('Sprites/Item Icons/Ingredients/gem', 'Edelstein des Mutes', 'erhöht Verteidigung permanent um 5', 'Usable', 0, 'PhysicalDefense,5,MagicDefense,5', 0, 3),
('Sprites/Item Icons/Ingredients/gemGreen', 'Edelstein des Lebens', 'erhöht Lebenspunkte permanent um 100', 'Usable', 0, 'Health, 100', 0, 3),
('Sprites/Item Icons/Ingredients/gemRed', 'Edelstein des Furors', 'erhöht kritische Trefferchance permanent um 1%', 'Usable', 0, 'CritChance,1', 0, 3),
('Sprites/Item Icons/Ingredients/heart', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Ingredients/scroll', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Ingredients/tome', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion1', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion10', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion11', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion12', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion13', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion14', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion2', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion3', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion4', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion5', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion6', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion7', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion8', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Potions/potion9', 'item1', 'smth', 'Usable', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active10', 'item1', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active11', 'item1', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active12', 'Heldentum', 'erhöht physischen schaden um 30 für 10 sek.', 'Active', 600, 'MinDmg, 30, MaxDmg, 30', 10, 3),
('Sprites/Item Icons/Skills/active13', 'Schild', 'erhöht physische verteidigung +50 für 10 sekunden', 'Active', 100, 'PhysicalDefense, 50', 10, 3),
('Sprites/Item Icons/Skills/active14', 'item1', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active15', 'katzenreflexe', 'erhöht Ausweichchance um 25% für 10 sek.', 'Active', 100, 'DodgeChance, 25', 10, 3),
('Sprites/Item Icons/Skills/active2', 'magischer Durchbruch', 'ignoriert 50 magische verteidigung für 10 sek.', 'Active', 1000, 'MinMDmg,50,MaxMDmg,50', 10, 3),
('Sprites/Item Icons/Skills/active3', 'Tödliche präzision', 'erhöhte kritische trefferchance um 25% für 10 sekunden', 'Active', 100, 'CritChance,25', 10, 3),
('Sprites/Item Icons/Skills/active4', 'An', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active5', 'item1', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active6', 'item1', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active7', 'item1', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active8', 'item1', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/active9', 'item1', 'smth', 'Active', 100, 'smth', 10, 3),
('Sprites/Item Icons/Skills/passive1', 'smth', 'desc', 'Passive', 100, 'smth', 10, 0),
('Sprites/Item Icons/Skills/passive2', 'smth', 'desc', 'Passive', 100, 'smth', 10, 0),
('Sprites/Item Icons/Skills/passive3', 'smth', 'desc', 'Passive', 100, 'smth', 10, 0),
('Sprites/Item Icons/Skills/passive4', 'Kraft', 'erhöht physischen schaden um 30%', 'Passive', 100, 'DamageM, 1.3', 0, 0),
('Sprites/Item Icons/Weapons/Daggers/dagger1', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger10', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger2', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger3', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger4', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger5', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger6', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger7', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger8', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Daggers/dagger9', 'name', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/axe', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/axe2', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/axe3', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/axe4', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/axe5', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/axe6', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/Bomb', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/bow', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/bow2', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/dagger', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/dagger2', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/hammer', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/hammer2', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/sword', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/sword2', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/sword3', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/sword4', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/sword5', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/sword6', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/wand', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3),
('Sprites/Item Icons/Weapons/Other/wand2', 'smth', 'desc', 'Weapon', 100, 'smth', 10, 3);

-- --------------------------------------------------------

--
-- Table structure for table `playerdetails`
--

CREATE TABLE `playerdetails` (
  `username` varchar(16) NOT NULL,
  `Avatarname` varchar(16) NOT NULL,
  `AvatarID` varchar(100) NOT NULL,
  `FirstLogin` tinyint(1) NOT NULL DEFAULT '0',
  `Badge1` tinyint(1) NOT NULL DEFAULT '0',
  `Badge2` tinyint(1) NOT NULL DEFAULT '0',
  `Badge3` tinyint(1) NOT NULL DEFAULT '0',
  `ActivityStreak` float NOT NULL DEFAULT '1',
  `ActivityPoints` int(10) NOT NULL,
  `ActivityPointsGained` int(10) NOT NULL,
  `Level` int(10) NOT NULL,
  `XP` int(100) NOT NULL,
  `FreeStatPoints` int(10) NOT NULL,
  `Gold` int(10) NOT NULL,
  `BestStageBeaten` int(10) NOT NULL,
  `Stages` varchar(100) NOT NULL,
  `Inventory` varchar(300) NOT NULL,
  `Equipment` varchar(300) NOT NULL,
  `Stats` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `playerdetails`
--

INSERT INTO `playerdetails` (`username`, `Avatarname`, `AvatarID`, `FirstLogin`, `Badge1`, `Badge2`, `Badge3`, `ActivityStreak`, `ActivityPoints`, `ActivityPointsGained`, `Level`, `XP`, `FreeStatPoints`, `Gold`, `BestStageBeaten`, `Stages`, `Inventory`, `Equipment`, `Stats`) VALUES
('Bob', 'Bob', 'WarrGirl3', 0, 1, 1, 1, 1, 45, 0, 3, 25, 5, 548, 2, '3,1,0,0,0,0,0,0,0,0,0,0,0,0,', 'Magische Meeresmuschel,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Elsa', 'Elsa', 'Knight2', 0, 1, 1, 0, 1, 40, 0, 2, 50, 5, 543, 1, '3,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Jim', 'Jim', 'WarrGirl2', 0, 0, 0, 1, 1, 40, 0, 2, 75, 0, 141, 2, '3,1,0,0,0,0,0,0,0,0,0,0,0,0,', 'magischer Pilz,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'katzenreflexe,Kraft,NULL,NULL,', 'Strength,10,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,15,MaxDmg,60,PhysicalDefense,15,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Link', 'Link', 'AdvGirl', 0, 0, 0, 0, 1, 30, 0, 2, 100, 0, 573, 1, '3,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,10,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,15,MaxDmg,60,PhysicalDefense,20,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Longaria', 'Longaria', 'Elf1', 0, 0, 0, 0, 1, 50, 0, 1, 0, 0, 500, 0, '0,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Luigi', 'Luigi', 'Elf2', 0, 0, 0, 0, 1, 45, 0, 2, 25, 5, 513, 1, '3,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Mario', 'Mario', 'Knight3', 0, 0, 0, 0, 1, 45, 0, 2, 25, 5, 534, 1, '3,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Peach', 'Peach', 'fairy1', 0, 0, 0, 0, 1, 45, 0, 2, 25, 5, 546, 1, '3,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Samus', 'Samus', '', 1, 0, 0, 0, 1, 0, 50, 1, 0, 0, 500, 0, '', '', '', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Tim', 'Tim', 'WarrGirl1', 0, 0, 0, 0, 1, 35, 0, 2, 25, 0, 134, 1, '3,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'Schild,Kraft,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,10,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,15,MaxMDmg,40,MagicDefense,15,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Toad', 'Toad', 'Elf3', 0, 0, 0, 0, 1, 45, 0, 2, 25, 5, 544, 1, '3,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'Magische Meeresmuschel,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,'),
('Zelda', 'Zelda', 'NinjaGirl', 0, 0, 0, 0, 1, 45, 0, 2, 25, 5, 549, 1, '3,0,0,0,0,0,0,0,0,0,0,0,0,0,', 'NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,', 'NULL,NULL,NULL,NULL,', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,');

-- --------------------------------------------------------

--
-- Table structure for table `players`
--

CREATE TABLE `players` (
  `id` int(10) NOT NULL,
  `username` varchar(16) NOT NULL,
  `hash` varchar(100) NOT NULL,
  `salt` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `players`
--

INSERT INTO `players` (`id`, `username`, `hash`, `salt`) VALUES
(8, 'Longaria', '$5$rounds=5000$steamedhamsLonga$LIr50Pq0eivpNll/1UoqA/t3UEIWnHYs4zoEEmd2zAC', '$5$rounds=5000$steamedhamsLongaria$'),
(9, 'Tim', '$5$rounds=5000$steamedhamsTim$21jEmgCUnBcTKCNVzodOPPSpMTyEdjkI4lvcFb0hQh2', '$5$rounds=5000$steamedhamsTim$'),
(10, 'Jim', '$5$rounds=5000$steamedhamsJim$cwlqGeMogf5dFzsSg8slDpIOf9XuC0Fbw6.KPngC8h8', '$5$rounds=5000$steamedhamsJim$'),
(11, 'Bob', '$5$rounds=5000$steamedhamsBob$/El7BkupwYuY9PHu4mdw.FOrSPEMzYajvVZbh6rTnY4', '$5$rounds=5000$steamedhamsBob$'),
(12, 'Elsa', '$5$rounds=5000$steamedhamsElsa$pACXsO5JlRphAypN9vtR3/gwBQ2MDvycuU6xRrt5M34', '$5$rounds=5000$steamedhamsElsa$'),
(13, 'Mario', '$5$rounds=5000$steamedhamsMario$jJTHojEfvQ.k4VRbo6684eo71azFbZGik7BahOj2xYC', '$5$rounds=5000$steamedhamsMario$'),
(14, 'Luigi', '$5$rounds=5000$steamedhamsLuigi$h6Y0qoQhciUz38l356NMZwiXiijFtOGiwBe2cHbrwE2', '$5$rounds=5000$steamedhamsLuigi$'),
(15, 'Toad', '$5$rounds=5000$steamedhamsToad$V26L.38tPKuw00cPkJCciq4HdCRoIw43d9E/R2O2.xD', '$5$rounds=5000$steamedhamsToad$'),
(16, 'Peach', '$5$rounds=5000$steamedhamsPeach$ZMSZkkJGeanQEM5T4al2qaCDeA8H2QImGRXLvNKI4jB', '$5$rounds=5000$steamedhamsPeach$'),
(17, 'Link', '$5$rounds=5000$steamedhamsLink$OR/AXnCbUdPHEHdu40AGgwmnx8hiELr1OWxn4Zmktj0', '$5$rounds=5000$steamedhamsLink$'),
(18, 'Zelda', '$5$rounds=5000$steamedhamsZelda$yk9t5eibJyabGprgs/Tc0KBx.o9Zd3gOQ98HFmezQK5', '$5$rounds=5000$steamedhamsZelda$'),
(19, 'Samus', '$5$rounds=5000$steamedhamsSamus$ztvdz57zqPY4qME7kzQiD//7zt48iqpngKrrgcsnRe3', '$5$rounds=5000$steamedhamsSamus$');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `enemies`
--
ALTER TABLE `enemies`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `playerdetails`
--
ALTER TABLE `playerdetails`
  ADD PRIMARY KEY (`username`);

--
-- Indexes for table `players`
--
ALTER TABLE `players`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `players`
--
ALTER TABLE `players`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `playerdetails`
--
ALTER TABLE `playerdetails`
  ADD CONSTRAINT `playerdetails_ibfk_1` FOREIGN KEY (`username`) REFERENCES `players` (`username`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
