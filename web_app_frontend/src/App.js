import 'bootstrap/dist/css/bootstrap.min.css'
import Login from './Components/Login'
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import Dashboard from './Components/Dashboard';
import PrivateRoute from './Components/PrivateRoute';
import Home from './Components/Home';
import Department from './Components/Departments';
import Employees from './Components/Employees';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/login" element={<Login />} />
        <Route element={<PrivateRoute />}>
          <Route path="/dashboard" element={<Dashboard />}>
            <Route path="" element={<Home />} />
            <Route path='/dashboard/employee' element={<Employees />} />
            <Route path='/dashboard/departments' element={<Department />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
