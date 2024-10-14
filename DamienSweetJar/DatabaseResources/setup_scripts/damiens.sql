-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: damien's
-- ------------------------------------------------------
-- Server version	8.1.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `adresses`
--

DROP TABLE IF EXISTS `adresses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `adresses` (
  `AdressID` int NOT NULL,
  `Street` varchar(75) NOT NULL,
  `City` varchar(45) NOT NULL,
  `State` varchar(45) NOT NULL,
  `ZIPCode` varchar(45) NOT NULL,
  PRIMARY KEY (`AdressID`),
  KEY `STREET` (`Street`),
  KEY `CITY` (`City`),
  KEY `STATE` (`State`),
  KEY `ZIP` (`ZIPCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adresses`
--

LOCK TABLES `adresses` WRITE;
/*!40000 ALTER TABLE `adresses` DISABLE KEYS */;
INSERT INTO `adresses` VALUES (1,'742 Evergreen Terrace','Boston','Massachusetts','11111'),(2,'145 Daisy Lane','Kingston','New York','49238'),(3,'396 Maple Street','Nowhere','The Twilight Zone','98428'),(4,'2987 Leaf Road','Chinatown','Alabama','83798'),(5,'7884 Rose Street','New York City','New York','67328'),(6,'309 Reed Lane','New Street','Illinois','43927'),(7,'8923 Cowboy Lane','Metro City','Texas','10983'),(8,'6543 Acore Avenue','Brooklyn','New York','62843'),(9,'8490 King\'s Street','Farmville','Iowa','81912'),(10,'1475 Washington Avenue','Albany','New York','32032');
/*!40000 ALTER TABLE `adresses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cart`
--

DROP TABLE IF EXISTS `cart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cart` (
  `OrderID` int NOT NULL AUTO_INCREMENT,
  `CustomerID` int DEFAULT NULL,
  `AdressID` int DEFAULT NULL,
  `OrderDate` datetime DEFAULT NULL,
  `Total` float DEFAULT NULL,
  `CanBuy` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`OrderID`),
  KEY `fk_customer` (`CustomerID`) ,
  KEY `fk_canbuy_cookie_idx` (`CanBuy`),
  KEY `fk_adress_customer_idx` (`AdressID`),
  CONSTRAINT `fk_adress_customer` FOREIGN KEY (`AdressID`) REFERENCES `customers` (`AdressId`),
  CONSTRAINT `fk_canbuy_cookie` FOREIGN KEY (`CanBuy`) REFERENCES `cookie` (`IsInStock`),
  CONSTRAINT `fk_customer` FOREIGN KEY (`CustomerID`) REFERENCES `customers` (`CustomerID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `cartItems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cartItems` (
    `OrderID` int NOT NULL,
    `CookieID` int NOT NULL,
    `CookieAmount` int NOT NULL,
    CONSTRAINT `fk_orderID` FOREIGN KEY (`OrderID`) REFERENCES `cart` (`OrderID`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cart`
--

# LOCK TABLES `cart` WRITE;
# /*!40000 ALTER TABLE `cart` DISABLE KEYS */;
# INSERT INTO `cart` VALUES (1,1,2,1,1,'2003-06-12 00:00:00',5.99,1),(2,2,4,2,2,'2012-12-03 00:00:00',7.99,1),(3,3,6,3,3,'2023-12-01 00:00:00',20.99,0),(4,4,8,4,10,'2008-03-23 00:00:00',49.99,1),(5,5,10,5,5,'2018-07-14 00:00:00',14.99,0);
# /*!40000 ALTER TABLE `cart` ENABLE KEYS */;
# UNLOCK TABLES;

--
-- Table structure for table `cookie`
--

DROP TABLE IF EXISTS `cookie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cookie` (
  `CookieID` int NOT NULL,
  `CookieName` varchar(45) NOT NULL,
  `CookiePrice` float NOT NULL,
  `InStock` int NOT NULL,
  `IsInStock` tinyint NOT NULL DEFAULT '1',
#   `CookieIMG` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`CookieID`),
  KEY `CANBUY` (`IsInStock`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cookie`
--

LOCK TABLES `cookie` WRITE;
/*!40000 ALTER TABLE `cookie` DISABLE KEYS */;
INSERT INTO `cookie` VALUES (1,'Chocolate Chip',5.99, 12, 1),
                            (2,'Oatmeal',3.99, 20,1),(3,'M and M',6.99,0,0),
                            (4,'Gingerbread',4.99,30,1),(5,'Rasin',2.99,0,0),
                            (6,'Snickerdoodle',5.99,9,1),(7,'Sugar',4.99,5,1),
                            (8,'Gingersnaps',8.99,0,0),(9,'Shortbread',3.99,20,1),
                            (10,'Damien\'s Special',9.99,32,1);
/*!40000 ALTER TABLE `cookie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customers`
--

DROP TABLE IF EXISTS `customers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customers` (
  `CustomerID` int AUTO_INCREMENT NOT NULL,
  `PassWordHash` varchar(200) DEFAULT NULL,
  `Salt` varchar(45) DEFAULT NULL,
  `UserName` varchar(45) DEFAULT NULL, 
  `FName` varchar(45) DEFAULT NULL,
  `LName` varchar(45) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  `PhoneNumber` varchar(45) DEFAULT NULL,
  `PastOrderIds` int DEFAULT NULL,
  `AdressId` int DEFAULT NULL,
  PRIMARY KEY (`CustomerID`),
  KEY `ADRESS` (`AdressId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customers`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customers` DISABLE KEYS */;
INSERT INTO `customers` VALUES (1,'bhhhb','skdhfgdfk','userjohn','John','Smith','john@gmail.com','833-445-9090',NULL,2),
                               (2,'naka','uigdkfdfjg','userJane','Jane','Doe','doej@gmail.com','295-378-9237',NULL,4),
                               (3,'akion','uigdkfdfjg','userWill','Will','Sullivan','wills430@gmail.com','489-290-0219',NULL,6),
                               (4,'eujiae','uigdkfdfjg','userEugene','Eugene','Deyo','bige@gmail.com','123-456-7890',NULL,8),
                               (5,'dsniuhwfuiheuj','uigdkfdfjg','userSam','Sam','Wilkins','wilkinssam@outlook.com','982-238-3902',NULL,10);
/*!40000 ALTER TABLE `customers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `datetimekeeper`
--

DROP TABLE IF EXISTS `datetimekeeper`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `datetimekeeper` (
  `OrderID` int NOT NULL,
  `Date` date NOT NULL,
  `Time` time NOT NULL,
  `Year` year NOT NULL,
  PRIMARY KEY (`OrderID`),
  KEY `DATE` (`Date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `datetimekeeper`
--

LOCK TABLES `datetimekeeper` WRITE;
/*!40000 ALTER TABLE `datetimekeeper` DISABLE KEYS */;
INSERT INTO `datetimekeeper` VALUES (1,'2003-06-12','03:11:57',2003),(2,'2012-12-03','17:20:20',2012),(3,'2023-12-01','12:09:20',2023),(4,'2008-03-23','14:32:27',2008),(5,'2018-07-14','18:09:51',2018);
/*!40000 ALTER TABLE `datetimekeeper` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kitchen`
--

DROP TABLE IF EXISTS `kitchen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kitchen` (
  `KitchenID` int NOT NULL,
  `KitchenName` varchar(45) DEFAULT NULL,
  `KitchenStreet` varchar(75) DEFAULT NULL,
  `KitchenCity` varchar(45) DEFAULT NULL,
  `KitchenState` varchar(45) DEFAULT NULL,
  `KitchenZIP` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`KitchenID`),
  KEY `fk_street` (`KitchenStreet`),
  KEY `fk_city` (`KitchenCity`),
  KEY `fk_state` (`KitchenState`),
  KEY `fk_zip` (`KitchenZIP`),
  CONSTRAINT `fk_city` FOREIGN KEY (`KitchenCity`) REFERENCES `adresses` (`City`),
  CONSTRAINT `fk_state` FOREIGN KEY (`KitchenState`) REFERENCES `adresses` (`State`),
  CONSTRAINT `fk_street` FOREIGN KEY (`KitchenStreet`) REFERENCES `adresses` (`Street`),
  CONSTRAINT `fk_zip` FOREIGN KEY (`KitchenZIP`) REFERENCES `adresses` (`ZIPCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kitchen`
--

LOCK TABLES `kitchen` WRITE;
/*!40000 ALTER TABLE `kitchen` DISABLE KEYS */;
INSERT INTO `kitchen` VALUES (1,'Bakery Kitchen','742 Evergreen Terrace','Boston','Massachusetts','11111'),(3,'Dubious Kitchen','396 Maple Street','Nowhere','The Twilight Zone','98428'),(5,'Bloody Mary Bakery','7884 Rose Street','New York City','New York','67328'),(7,'Rodeo Kitchen','8923 Cowboy Lane','Metro City','Texas','10983'),(9,'Organic Kitchen','8490 King\'s Street','Farmville','Iowa','81912');
/*!40000 ALTER TABLE `kitchen` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-12-01 12:53:09
