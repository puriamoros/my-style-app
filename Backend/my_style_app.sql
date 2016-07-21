-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 21-07-2016 a las 19:58:08
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
  `date` datetime NOT NULL,
  `status` tinyint(4) NOT NULL,
  `notes` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `appointments`
--

INSERT INTO `appointments` (`id`, `idClient`, `idEstablishment`, `idService`, `date`, `status`, `notes`) VALUES
(1, 1, 1, 4, '2016-07-09 10:30:00', 1, 'asdf'),
(2, 1, 2, 22, '2016-07-04 15:30:00', 0, 'hola'),
(3, 1, 2, 24, '2016-07-04 16:00:00', 2, 'hola'),
(6, 1, 1, 4, '2016-07-08 18:00:00', 1, 'hola'),
(7, 1, 1, 1, '2016-07-10 11:00:00', 0, ''),
(8, 1, 1, 5, '2016-07-10 17:00:00', 2, ''),
(9, 1, 1, 1, '2016-07-10 13:00:00', 1, ''),
(10, 1, 1, 5, '2016-07-12 11:00:00', 0, ''),
(11, 1, 1, 5, '2016-07-11 11:00:00', 0, ''),
(12, 1, 4, 5, '2016-07-12 18:30:00', 0, ''),
(13, 1, 4, 5, '2016-07-12 16:30:00', 0, ''),
(14, 1, 4, 4, '2016-07-12 22:30:00', 0, ''),
(15, 1, 1, 4, '2016-07-14 17:00:00', 1, ''),
(16, 1, 1, 1, '2016-07-15 13:30:00', 1, ''),
(17, 1, 4, 21, '2016-07-18 17:30:00', 1, ''),
(18, 19, 1, 1, '2016-07-20 11:00:00', 1, ''),
(19, 19, 1, 1, '2016-07-20 10:00:00', 0, ''),
(20, 19, 1, 4, '2016-07-18 17:30:00', 2, ''),
(21, 1, 1, 5, '2016-07-28 17:30:00', 0, ''),
(22, 1, 1, 5, '2016-07-27 11:30:00', 1, ''),
(23, 1, 4, 5, '2016-07-20 18:30:00', 0, '');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `establishments`
--

DROP TABLE IF EXISTS `establishments`;
CREATE TABLE `establishments` (
  `id` int(11) NOT NULL,
  `name` varchar(200) NOT NULL,
  `address` text NOT NULL,
  `phone` varchar(9) NOT NULL,
  `idEstablishmentType` int(11) NOT NULL,
  `idOwner` int(11) NOT NULL,
  `idProvince` int(11) NOT NULL,
  `concurrence` int(11) NOT NULL,
  `hours1` varchar(11) NOT NULL,
  `hours2` varchar(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `establishments`
--

INSERT INTO `establishments` (`id`, `name`, `address`, `phone`, `idEstablishmentType`, `idOwner`, `idProvince`, `concurrence`, `hours1`, `hours2`) VALUES
(1, 'Peluquería 1', 'C/ Pintor Fernando Belda nº1, Bajo C', '958111111', 1, 19, 1, 1, '10:00-14:00', '16:00-20:00'),
(2, 'Peluquería 2', 'C/ Trajano nº2', '951357357', 2, 19, 1, 2, '08:00-20:00', ''),
(3, 'Peluquería 3', 'C/ Camino de Ronda nº3', '357159486', 1, 19, 2, 3, '06:00-15:00', ''),
(4, 'Peluquería 4', 'Carretera de Murcia nº4, bajo izq bajo bajo', '123456789', 3, 19, 1, 4, '16:00-00:00', '');

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

--
-- Volcado de datos para la tabla `favourites`
--

INSERT INTO `favourites` (`id`, `idClient`, `idEstablishment`) VALUES
(50, 1, 1),
(52, 19, 1),
(51, 19, 4);

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
(1, 1, 12.5),
(1, 4, 25.5),
(1, 5, 16),
(2, 22, 12.5),
(2, 23, 25.5),
(2, 24, 16),
(3, 1, 12.5),
(3, 3, 25.5),
(3, 4, 16),
(4, 1, 10.5),
(4, 2, 25.5),
(4, 3, 16),
(4, 4, 25.5),
(4, 5, 16),
(4, 21, 12.5),
(4, 22, 25.5),
(4, 23, 16),
(4, 24, 25.5),
(4, 25, 16);

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
  `idTranslation` int(11) NOT NULL,
  `idServiceCategory` int(11) NOT NULL,
  `duration` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `services`
--

INSERT INTO `services` (`id`, `idTranslation`, `idServiceCategory`, `duration`) VALUES
(1, 12, 1, 30),
(2, 13, 1, 30),
(3, 14, 1, 60),
(4, 15, 1, 60),
(5, 16, 1, 60),
(6, 17, 1, 60),
(7, 18, 2, 30),
(8, 19, 2, 30),
(9, 20, 2, 30),
(10, 21, 2, 30),
(11, 22, 2, 30),
(12, 23, 2, 30),
(13, 24, 3, 60),
(14, 25, 3, 60),
(15, 26, 3, 60),
(16, 27, 3, 60),
(17, 28, 3, 60),
(18, 29, 3, 60),
(19, 30, 4, 90),
(20, 31, 4, 90),
(21, 32, 5, 60),
(22, 33, 6, 30),
(23, 34, 6, 60),
(24, 35, 6, 30),
(25, 36, 6, 30),
(26, 37, 6, 30),
(27, 38, 6, 30),
(28, 39, 6, 30),
(29, 40, 6, 30),
(30, 41, 7, 60),
(31, 42, 7, 60),
(32, 43, 7, 60),
(33, 44, 8, 45),
(34, 45, 8, 30),
(35, 46, 8, 90),
(36, 47, 8, 30),
(37, 48, 8, 45),
(38, 49, 8, 60),
(39, 50, 9, 60),
(40, 51, 9, 60),
(41, 52, 10, 60),
(42, 53, 10, 60),
(43, 54, 10, 60),
(44, 55, 11, 45),
(45, 56, 11, 45),
(46, 57, 11, 45);

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
  `idTranslation` int(11) NOT NULL,
  `idEstablishmentType` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `service_categories`
--

INSERT INTO `service_categories` (`id`, `idTranslation`, `idEstablishmentType`) VALUES
(1, 1, 1),
(2, 2, 1),
(3, 3, 1),
(4, 4, 1),
(5, 5, 1),
(6, 6, 2),
(7, 7, 2),
(8, 8, 2),
(9, 9, 2),
(10, 10, 2),
(11, 11, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `slots`
--

DROP TABLE IF EXISTS `slots`;
CREATE TABLE `slots` (
  `id` int(11) NOT NULL,
  `date` date NOT NULL,
  `idAppointment` int(11) NOT NULL
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

--
-- Volcado de datos para la tabla `staff`
--

INSERT INTO `staff` (`idUser`, `idEstablishment`) VALUES
(20, 2),
(21, 2),
(23, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `translations`
--

DROP TABLE IF EXISTS `translations`;
CREATE TABLE `translations` (
  `id` int(11) NOT NULL,
  `en` text NOT NULL,
  `es` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `translations`
--

INSERT INTO `translations` (`id`, `en`, `es`) VALUES
(1, 'Hair Styles', 'Peluquería'),
(2, 'Haircuts', 'Cortes'),
(3, 'Colouring and streaks', 'Coloración y mechas'),
(4, 'Permanents', 'Permanentes'),
(5, 'Hair extensions', 'Extensiones'),
(6, 'Hair removal', 'Depilación'),
(7, 'Hands and feet', 'Belleza manos-pies'),
(8, 'Eyes', 'Belleza ojos'),
(9, 'Facial', 'Facial'),
(10, 'Facial laser', 'Láser facial'),
(11, 'Makeups', 'Maquillajes'),
(12, 'Short hairstyle', 'Peinar corto'),
(13, 'Medium hairstyle', 'Peinar mediano'),
(14, 'Long hairstyle', 'Peinar largo'),
(15, 'Half tied back', 'Semirecogido'),
(16, 'Tied back', 'Recogido'),
(17, 'Braided', 'Trenzados completos'),
(18, 'Lady cut', 'Corte señora'),
(19, 'Gentleman cut', 'Corte caballero'),
(20, 'Hair clipper cut', 'Maquinilla'),
(21, 'Child cut', 'Corte infantil'),
(22, 'Fringe', 'Retoque flequillo'),
(23, 'Beard', 'Arreglo barba'),
(24, 'Hair colouring', 'Coloración (Tinte o baño de color)'),
(25, 'Single colour streaks', 'Mechas enteras'),
(26, 'Double colour streaks', 'Mechas enteras bicolor'),
(27, '1/2 Single colour streaks', '1/2 Mechas'),
(28, '1/2 Double colour streaks', '1/2 Mechas bicolor'),
(29, 'Californian streaks', 'Mechas californianas'),
(30, 'Japanese thermal reconditioning', 'Alisado Japonés'),
(31, 'Fluffy hair', 'Moldeador o ahuecador'),
(32, 'Hair extensions', 'Colocación extensiones de clip'),
(33, 'Half legs', 'Medias piernas tibias o calientes'),
(34, 'Full legs', 'Piernas enteras tibias o calientes'),
(35, 'Chest or back', 'Pecho o espalda'),
(36, 'Arms', 'Brazos'),
(37, 'Groin', 'Ingles'),
(38, 'Brazilian wax', 'Ingles brasileñas'),
(39, 'Underarm', 'Axilas'),
(40, 'Superior lip', 'Labio superior'),
(41, 'Nails permanent makeup', 'Maquillado permanente de uñas manos o pies'),
(42, 'Manicure', 'Manicura'),
(43, 'Pedicure', 'Pedicura'),
(44, 'Eyelash perm', 'Permanente de pestañas'),
(45, 'Eyelash colouring', 'Tinte de pestañas'),
(46, 'Eyelash extensions', 'Extensiones de pestañas'),
(47, 'Eyebrows colouring', 'Tinte de cejas'),
(48, 'Eyelash extension setting', 'Aplicación de pestañas postizas'),
(49, 'Eyelash perm + colouring', 'Permanente + Tinte de pestañas'),
(50, 'Facial cleaning', 'Higiene facial completa'),
(51, 'Facial exfoliation and hydration', 'Exfoliación e hidratación facial'),
(52, 'Photo rejuvenation', 'Fotorejuvenecimiento'),
(53, 'Facial blemishes', 'Manchas faciales'),
(54, 'Acne', 'Acné'),
(55, 'Social makeup', 'Maquillaje social'),
(56, 'Party makeup', 'Maquillaje fiesta'),
(57, 'Carnival or fantasy makeup', 'Maquillaje carnaval o fantasía');

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
  `phone` varchar(9) NOT NULL,
  `platform` varchar(20) NOT NULL,
  `pushToken` varchar(300) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `users`
--

INSERT INTO `users` (`id`, `name`, `surname`, `email`, `password`, `apiKey`, `userType`, `phone`, `platform`, `pushToken`) VALUES
(1, 'Helio', 'Huete López de las Huertas', 'helio.huete@gmail.com', '$2y$10$HcPK6svqWeNqsMlcGNx.Ce0GR0mvq/QvfUTH0nXUwpR.jc660MSa6', 'cc06c9f321e156c2468669728e2be8b8', 1, '123456789', 'Windows', 'https://db5.notify.windows.com/?token=AwYAAABzlV1f%2b%2f1MR9qoNkd2Wgqp%2fkMNG6svunv2TqadYGMbyRvHcqTvJEgQ1oFjrzodqDGEHU4WjguX3TMhym5JzwCCMPDOd45Cc6d%2b2Jq1KjAB8QQsQuG7uK2fyLVJXh%2fx8ds%3d'),
(19, 'Puri', 'Amorós', 'puri.amoros@gmail.com', '$2y$10$AZfna1yNJg.NEjPjpNU/mefD4XuNoVD4k2cc7ojv0W5MIafTk8rV.', '3bc23b47841173b7027d911bc055d113', 2, '987654321', 'Windows', 'https://db5.notify.windows.com/?token=AwYAAADd1N6GI%2fKAWrHkR4BWoC%2fMxi3GiOrevf7ku6Htilf%2bOTE%2bUve70nm5O9xJqtfgJINzxffmFjBEUSOXmfR5oHMw2FdBWypSAdzo%2baBBFQ1bpjWQXvOjGFt0nkX%2fsQ0EXOY%3d'),
(20, 'Marta', 'Asdf', 'marta@gmail.com', '$2y$10$r1NGrTmPIZqvjaO.3nGaQO8g4ovt.EiX3TKcSHh1Yh4PQAEh2Ef/W', 'f166df0a695856c42e1827a31f90f947', 1, '123456789', '', ''),
(21, 'sdfasdf', 'asdfasdfdf', 'piunchi@gmail.com', '$2y$10$W.nmnDfq4A.fyoLz89d9U.R2eWcbdQ7WabRNYz5RlZ2tgSVDbLFnq', 'cb96faa601c5951c670e7894502267e3', 3, '123456789', '', ''),
(23, 'Antonio', 'Perez', 'antonio1.perez23@gmail.com', '$2y$10$yVus9HtHwcC152DGkB0PdeCPG5GTC10O9qHLtuud.0AQpyHt3lBWe', 'c0d99b5c244bb84712560dfaa56e5f7e', 4, '958123456', '', '');

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
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `idClient` (`idClient`,`idEstablishment`);

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
-- Indices de la tabla `slots`
--
ALTER TABLE `slots`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `staff`
--
ALTER TABLE `staff`
  ADD PRIMARY KEY (`idUser`,`idEstablishment`);

--
-- Indices de la tabla `translations`
--
ALTER TABLE `translations`
  ADD PRIMARY KEY (`id`);

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;
--
-- AUTO_INCREMENT de la tabla `establishments`
--
ALTER TABLE `establishments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT de la tabla `establishment_types`
--
ALTER TABLE `establishment_types`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT de la tabla `favourites`
--
ALTER TABLE `favourites`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;
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
-- AUTO_INCREMENT de la tabla `slots`
--
ALTER TABLE `slots`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
