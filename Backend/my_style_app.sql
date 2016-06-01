-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 01-06-2016 a las 12:00:04
-- Versión del servidor: 10.1.13-MariaDB
-- Versión de PHP: 5.6.21

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `my_style_app`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `appointments`
--

DROP TABLE IF EXISTS `appointments`;
CREATE TABLE `appointments` (
  `id` int(11) NOT NULL,
  `idClient` int(11) NOT NULL,
  `idEstablishment` int(11) NOT NULL,
  `idService` int(11) NOT NULL,
  `date` varchar(100) NOT NULL,
  `notes` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `appointments`
--

INSERT INTO `appointments` (`id`, `idClient`, `idEstablishment`, `idService`, `date`, `notes`) VALUES
(1, 2, 3, 3, 'rrrrr', 'aaaaaaaaaaaa');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `establishments`
--

DROP TABLE IF EXISTS `establishments`;
CREATE TABLE `establishments` (
  `id` int(11) NOT NULL,
  `name` varchar(200) NOT NULL,
  `address` text NOT NULL,
  `establishmentType` int(11) NOT NULL,
  `idOwner` int(11) NOT NULL,
  `idProvince` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `establishments`
--

INSERT INTO `establishments` (`id`, `name`, `address`, `establishmentType`, `idOwner`, `idProvince`) VALUES
(4, 'peluquería asdf', 'C/. Tururu nº4 asdf', 0, 0, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `favourites`
--

DROP TABLE IF EXISTS `favourites`;
CREATE TABLE `favourites` (
  `id` int(11) NOT NULL,
  `idClient` int(11) NOT NULL,
  `idEstablishment` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `favourites`
--

INSERT INTO `favourites` (`id`, `idClient`, `idEstablishment`) VALUES
(2, 1, 5),
(3, 19, 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `offer`
--

DROP TABLE IF EXISTS `offer`;
CREATE TABLE `offer` (
  `idEstablishment` int(11) NOT NULL,
  `idService` int(11) NOT NULL,
  `price` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `offer`
--

INSERT INTO `offer` (`idEstablishment`, `idService`, `price`) VALUES
(4, 3, 12.5),
(4, 4, 25.5),
(4, 5, 16);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `provinces`
--

DROP TABLE IF EXISTS `provinces`;
CREATE TABLE `provinces` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `services`
--

DROP TABLE IF EXISTS `services`;
CREATE TABLE `services` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `idServiceCategory` int(11) NOT NULL,
  `duration` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `services`
--

INSERT INTO `services` (`id`, `name`, `idServiceCategory`, `duration`) VALUES
(1, 'corte caballero', 0, 30);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `services_history`
--

DROP TABLE IF EXISTS `services_history`;
CREATE TABLE `services_history` (
  `idClient` int(11) NOT NULL,
  `idEstablishment` int(11) NOT NULL,
  `idService` int(11) NOT NULL,
  `date` varchar(50) NOT NULL,
  `notes` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `service_categories`
--

DROP TABLE IF EXISTS `service_categories`;
CREATE TABLE `service_categories` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `service_types`
--

DROP TABLE IF EXISTS `service_types`;
CREATE TABLE `service_types` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `staff`
--

DROP TABLE IF EXISTS `staff`;
CREATE TABLE `staff` (
  `idUser` int(11) NOT NULL,
  `idEstablishment` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `surname` varchar(100) NOT NULL,
  `email` varchar(200) NOT NULL,
  `password` varchar(100) NOT NULL,
  `apiKey` varchar(100) NOT NULL,
  `userType` int(11) NOT NULL,
  `phone` varchar(9) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `users`
--

INSERT INTO `users` (`id`, `name`, `surname`, `email`, `password`, `apiKey`, `userType`, `phone`) VALUES
(1, 'Helio', 'Huete', 'helio.huete@gmail.com', '$2y$10$g0v8y38VjKujbzPQCa9g.e2HeYt5fLAC5o1asOxoqmvN53GvbReg6', 'cc06c9f321e156c2468669728e2be8b8', 0, '0'),
(19, 'Puri', 'Amorós', 'puri.amoros@gmail.com', '$2y$10$/kXT7NWAulGrX31H/BNLmOQfFr41znROhIxlDDuKlA1wU6mWHINxO', '3bc23b47841173b7027d911bc055d113', 0, '0');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `appointments`
--
ALTER TABLE `appointments`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `establishments`
--
ALTER TABLE `establishments`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `favourites`
--
ALTER TABLE `favourites`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `offer`
--
ALTER TABLE `offer`
  ADD PRIMARY KEY (`idEstablishment`,`idService`);

--
-- Indices de la tabla `provinces`
--
ALTER TABLE `provinces`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `services`
--
ALTER TABLE `services`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `services_history`
--
ALTER TABLE `services_history`
  ADD PRIMARY KEY (`idClient`,`idEstablishment`,`idService`);

--
-- Indices de la tabla `service_categories`
--
ALTER TABLE `service_categories`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `service_types`
--
ALTER TABLE `service_types`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `staff`
--
ALTER TABLE `staff`
  ADD PRIMARY KEY (`idUser`,`idEstablishment`);

--
-- Indices de la tabla `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `userEmail` (`email`),
  ADD UNIQUE KEY `userApiKey` (`apiKey`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `appointments`
--
ALTER TABLE `appointments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT de la tabla `establishments`
--
ALTER TABLE `establishments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT de la tabla `favourites`
--
ALTER TABLE `favourites`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT de la tabla `provinces`
--
ALTER TABLE `provinces`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `services`
--
ALTER TABLE `services`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT de la tabla `service_categories`
--
ALTER TABLE `service_categories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `service_types`
--
ALTER TABLE `service_types`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
