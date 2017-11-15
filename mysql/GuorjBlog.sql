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


-- Dumping database structure for guorjblog
DROP DATABASE IF EXISTS `guorjblog`;
CREATE DATABASE IF NOT EXISTS `guorjblog` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `guorjblog`;

-- Dumping structure for table guorjblog.comments
CREATE TABLE IF NOT EXISTS `comments` (
  `ID` char(36) NOT NULL,
  `Content` varchar(2000) NOT NULL,
  `ContentID` char(36) NOT NULL,
  `CreateAt` datetime(6) NOT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `ParentCommentID` char(36) NOT NULL,
  `PostID` char(36) NOT NULL,
  `UserEmail` varchar(256) DEFAULT NULL,
  `UserID` char(36) DEFAULT NULL,
  `UserName` varchar(16) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_Comments_PostID` (`PostID`),
  CONSTRAINT `FK_Comments_Posts_PostID` FOREIGN KEY (`PostID`) REFERENCES `posts` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Data exporting was unselected.
-- Dumping structure for table guorjblog.postcontents
CREATE TABLE IF NOT EXISTS `postcontents` (
  `ID` char(36) NOT NULL,
  `MD5Hash` varchar(32) NOT NULL,
  `Content` longtext NOT NULL,
  `CreateAt` datetime(6) NOT NULL,
  `EditorType` tinyint(4) NOT NULL,
  `PostID` char(36) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_PostContents_PostID` (`PostID`),
  CONSTRAINT `FK_PostContents_Posts_PostID` FOREIGN KEY (`PostID`) REFERENCES `posts` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Data exporting was unselected.
-- Dumping structure for table guorjblog.posts
CREATE TABLE IF NOT EXISTS `posts` (
  `ID` char(36) NOT NULL,
  `CurrentContentID` char(36) NOT NULL,
  `UserID` char(36) NOT NULL,
  `CreateAt` datetime(6) NOT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `Summary` varchar(1000) DEFAULT NULL,
  `Title` varchar(256) NOT NULL,
  `URL` varchar(256) DEFAULT NULL,
  `UpdateAt` datetime(6) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Data exporting was unselected.
-- Dumping structure for table guorjblog.posttags
CREATE TABLE IF NOT EXISTS `posttags` (
  `ID` char(36) NOT NULL,
  `PostID` char(36) NOT NULL,
  `Tag` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_PostTags_PostID` (`PostID`),
  CONSTRAINT `FK_PostTags_Posts_PostID` FOREIGN KEY (`PostID`) REFERENCES `posts` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
