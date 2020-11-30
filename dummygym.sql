-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 03, 2019 at 03:21 AM
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
-- Database: `dummygym`
--

-- --------------------------------------------------------

--
-- Table structure for table `courses`
--

CREATE TABLE `courses` (
  `courseID` int(11) NOT NULL,
  `courseName` varchar(100) NOT NULL,
  `Dates` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `courses`
--

INSERT INTO `courses` (`courseID`, `courseName`, `Dates`) VALUES
(3, 'Test', '2019-05-16 13:33:00');

-- --------------------------------------------------------

--
-- Table structure for table `participating`
--

CREATE TABLE `participating` (
  `courseID` int(100) NOT NULL,
  `UserID` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `participating`
--

INSERT INTO `participating` (`courseID`, `UserID`) VALUES
(3, 'Bob'),
(3, 'Elsa'),
(3, 'Jim'),
(3, 'Link'),
(3, 'Longaria'),
(3, 'Luigi'),
(3, 'Mario'),
(3, 'Peach'),
(3, 'Tim'),
(3, 'Toad'),
(3, 'Zelda'),
(3, 'Samus');

-- --------------------------------------------------------

--
-- Table structure for table `userbase`
--

CREATE TABLE `userbase` (
  `id` int(10) NOT NULL,
  `Vorname` varchar(16) NOT NULL,
  `Name` varchar(16) NOT NULL,
  `Username` varchar(16) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `userbase`
--

INSERT INTO `userbase` (`id`, `Vorname`, `Name`, `Username`) VALUES
(1, 'Niko', 'Schmidt', 'Longaria'),
(13, 'fgdfgdhfg', 'dfhdfhdf', 'Tim'),
(14, 'fgserfgsrsg', 'sregserg', 'Jim'),
(15, 'sdfsfrewtghg', 'hjfgfgjh', 'Bob'),
(16, 'sdffffff', 'ffdsfddf', 'Elsa'),
(17, 'sdgfgs', 'sdgdsgg', 'Mario'),
(18, 'adfasd', 'sdfsdfsdf', 'Luigi'),
(19, 'asfafdsf', 'fsdff', 'Toad'),
(20, 'sdfsdfsdf', 'sdfsdfsdfqa', 'Peach'),
(21, 'ghsfghsgh', 'sgfhsgfhsfgh', 'Link'),
(22, 'srhstrhsth', 'sthstrh', 'Zelda'),
(23, 'sdfsdfsdfhhh', 'ggggdfd', 'Samus');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `courses`
--
ALTER TABLE `courses`
  ADD PRIMARY KEY (`courseID`);

--
-- Indexes for table `participating`
--
ALTER TABLE `participating`
  ADD KEY `UserID` (`UserID`),
  ADD KEY `courseID` (`courseID`) USING BTREE;

--
-- Indexes for table `userbase`
--
ALTER TABLE `userbase`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `Username` (`Username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `courses`
--
ALTER TABLE `courses`
  MODIFY `courseID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `userbase`
--
ALTER TABLE `userbase`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `participating`
--
ALTER TABLE `participating`
  ADD CONSTRAINT `participating_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `userbase` (`Username`),
  ADD CONSTRAINT `participating_ibfk_2` FOREIGN KEY (`courseID`) REFERENCES `courses` (`courseID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
