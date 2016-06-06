-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 06-06-2016 a las 10:48:18
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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `establishments`
--

DROP TABLE IF EXISTS `establishments`;
CREATE TABLE `establishments` (
  `id` int(11) NOT NULL,
  `name` varchar(200) NOT NULL,
  `address` text NOT NULL,
  `idEstablishmentType` int(11) NOT NULL,
  `idOwner` int(11) NOT NULL,
  `idProvince` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `establishment_types`
--

DROP TABLE IF EXISTS `establishment_types`;
CREATE TABLE `establishment_types` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `establishment_types`
--

INSERT INTO `establishment_types` (`id`, `name`) VALUES
(1, 'Peluquería'),
(2, 'Estética'),
(3, 'Peluquería y Estética');

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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `provinces`
--

DROP TABLE IF EXISTS `provinces`;
CREATE TABLE `provinces` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `provinces`
--

INSERT INTO `provinces` (`id`, `name`) VALUES
(1, 'Álava'),
(2, 'Albacete'),
(3, 'Alicante'),
(4, 'Almería '),
(5, 'Asturias '),
(6, 'Ávila '),
(7, 'Badajoz '),
(8, 'Barcelona '),
(9, 'Burgos '),
(10, 'Cáceres '),
(11, 'Cádiz '),
(12, 'Cantabria '),
(13, 'Castellón '),
(14, 'Ciudad Real'),
(15, 'Córdoba '),
(16, 'Cuenca '),
(17, 'Gerona '),
(18, 'Granada '),
(19, 'Guadalajara '),
(20, 'Guipúzcoa '),
(21, 'Huelva '),
(22, 'Huesca '),
(23, 'Islas Baleares'),
(24, 'Jaén '),
(25, 'La Coruña'),
(26, 'La Rioja'),
(27, 'Las Palmas'),
(28, 'León '),
(29, 'Lérida '),
(30, 'Lugo '),
(31, 'Madrid '),
(32, 'Málaga '),
(33, 'Murcia '),
(34, 'Navarra '),
(35, 'Orense '),
(36, 'Palencia '),
(37, 'Pontevedra '),
(38, 'Salamanca '),
(39, 'Segovia '),
(40, 'Sevilla '),
(41, 'Soria'),
(42, 'Tarragona '),
(43, 'Tenerife '),
(44, 'Teruel '),
(45, 'Toledo '),
(46, 'Valencia '),
(47, 'Valladolid '),
(48, 'Vizcaya '),
(49, 'Zamora '),
(50, 'Zaragoza ');

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
(1, 'Peinar corto', 1, 30),
(2, 'Peinar mediano', 1, 30),
(3, 'Peinar largo', 1, 45),
(4, 'Semirecogido', 1, 45),
(5, 'Recogido', 1, 60),
(6, 'Trenzados completos', 1, 45),
(7, 'Corte señora', 2, 30),
(8, 'Corte caballero', 2, 30),
(9, 'Maquinilla', 2, 30),
(10, 'Corte infantil', 2, 30),
(11, 'Retoque flequillo', 2, 15),
(12, 'Arreglo barba', 2, 15),
(13, 'Coloración (Tinte o baño de color)', 3, 60),
(14, 'Mechas enteras', 3, 60),
(15, 'Mechas enteras bicolor', 3, 60),
(16, '1/2 Mechas', 3, 45),
(17, '1/2 Mechas bicolor', 3, 45),
(18, 'Mechas californianas', 3, 60),
(19, 'Alisado Japonés', 4, 90),
(20, 'Moldeador o ahuecador', 4, 90),
(21, 'Colocación extensiones de clip', 5, 45),
(22, 'Medias piernas tibias o calientes', 6, 30),
(23, 'Piernas enteras tibias o calientes', 6, 45),
(24, 'Pecho o espalda', 6, 30),
(25, 'Brazos', 6, 30),
(26, 'Ingles', 6, 30),
(27, 'Ingles brasileñas ', 6, 30),
(28, 'Axilas', 6, 30),
(29, 'Labio superior', 6, 30),
(30, 'Maquillado permanente de uñas manos o pies', 7, 60),
(31, 'Manicura', 7, 60),
(32, 'Pedicura', 7, 60),
(33, 'Permanente de pestañas', 8, 45),
(34, 'Tinte de pestañas', 8, 30),
(35, 'Extensiones de pestañas', 8, 90),
(36, 'Tinte de cejas', 8, 30),
(37, 'Aplicación de pestañas postizas', 8, 45),
(38, 'Permanente + Tinte de pestañas', 8, 60),
(39, 'Higiene facial completa', 9, 60),
(40, 'Exfoliación e hidratación facial', 9, 60),
(41, 'Fotorejuvenecimiento', 10, 60),
(42, 'Manchas faciales', 10, 60),
(43, 'Acné', 10, 60),
(44, 'Maquillaje social', 11, 45),
(45, 'Maquillaje fiesta', 11, 45),
(46, 'Maquillaje carnaval o fantasía', 11, 45);

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
  `name` varchar(100) NOT NULL,
  `idEstablishmentType` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `service_categories`
--

INSERT INTO `service_categories` (`id`, `name`, `idEstablishmentType`) VALUES
(1, 'Peinados', 1),
(2, 'Cortes', 1),
(3, 'Coloración y mechas', 1),
(4, 'Permanentes', 1),
(5, 'Extensiones', 1),
(6, 'Depilación', 2),
(7, 'Belleza manos-pies', 2),
(8, 'Belleza ojos', 2),
(9, 'Facial', 2),
(10, 'Láser facial', 2),
(11, 'Maquillajes', 2);

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
-- Indices de la tabla `establishment_types`
--
ALTER TABLE `establishment_types`
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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `establishments`
--
ALTER TABLE `establishments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `establishment_types`
--
ALTER TABLE `establishment_types`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT de la tabla `favourites`
--
ALTER TABLE `favourites`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `provinces`
--
ALTER TABLE `provinces`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=51;
--
-- AUTO_INCREMENT de la tabla `services`
--
ALTER TABLE `services`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=47;
--
-- AUTO_INCREMENT de la tabla `service_categories`
--
ALTER TABLE `service_categories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT de la tabla `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
