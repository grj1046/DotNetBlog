-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.2.7-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for dotnetblog
DROP DATABASE IF EXISTS `dotnetblog`;
CREATE DATABASE IF NOT EXISTS `dotnetblog` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `dotnetblog`;

-- Dumping structure for table dotnetblog.accounts
CREATE TABLE IF NOT EXISTS `accounts` (
  `AccountID` char(36) NOT NULL,
  `UserName` varchar(16) DEFAULT NULL,
  `PhoneNumber` char(11) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `Salt` varchar(5) DEFAULT NULL,
  `PasswordHash` char(32) DEFAULT NULL,
  `UserID` char(36) DEFAULT NULL,
  `CreateAt` datetime(6) DEFAULT NULL,
  `UpdateAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`AccountID`),
  UNIQUE KEY `IX_Accounts_UserID` (`UserID`),
  CONSTRAINT `FK_Accounts_Users_UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Data exporting was unselected.
-- Dumping structure for table dotnetblog.users
CREATE TABLE IF NOT EXISTS `users` (
  `ID` char(36) NOT NULL,
  `Birthday` date DEFAULT NULL,
  `Gender` tinyint(4) DEFAULT NULL,
  `NickName` varchar(16) DEFAULT NULL,
  `CreateAt` datetime(6) DEFAULT NULL,
  `UpdateAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
