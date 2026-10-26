import { BrowserRouter, Routes, Route, Link, Navigate } from "react-router-dom";
import { AppBar, Toolbar, Button, Container } from "@mui/material";
import RoomsPage from "./pages/RoomsPage";
import UsersPage from "./pages/UsersPage";
import ReservationsPage from "./pages/ReservationsPage";

export default function App(){
  return (
    <BrowserRouter>
    <AppBar position="static">
      <Toolbar>
        {}
        <Button color="inherit" component={Link} to="/rooms">Rooms</Button>
        <Button color="inherit" component={Link} to="/users">Users</Button>
        <Button color="inherit" component={Link} to="/reservations">Reservations</Button>
      </Toolbar>
    </AppBar>

    <Container sx={{ mt: 3 }}>
      <Routes>
        <Route path="/" element={<Navigate to="/rooms" />} />
        <Route path="/rooms" element={<RoomsPage />} />
        <Route path="/users" element={<UsersPage />} />
        <Route path="/reservations" element={<ReservationsPage />} />
      </Routes>
    </Container>
    </BrowserRouter>
  )
}