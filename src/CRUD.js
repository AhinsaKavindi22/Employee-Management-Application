import React, { Fragment, useState, useEffect } from "react";
import Table from 'react-bootstrap/Table';
import "bootstrap/dist/css/bootstrap.min.css";
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios from 'axios';
import { ToastContainer, toast } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css'


const CRUD = () => {
    const [showEdit, setShowEdit] = useState(false);
    const [showCreate, setShowCreate] = useState(false);

    const handleCloseEdit = () => setShowEdit(false);
    const handleCloseCreate = () => setShowCreate(false);
    const handleShowEdit = () => setShowEdit(true);
    const handleShowCreate = () => setShowCreate(true);

    const [name,setName] = useState('')
    const [age,setAge] = useState('')
    const [isActive,setIsActive] = useState(0)

    const [editId,setEditId] = useState('')
    const [editName,setEditName] = useState('')
    const [editAge,setEditAge] = useState('')
    const [editIsActive,setEditIsActive] = useState(0)


    // const empdata = [
    //     { id: 1, name: "Ahinsa", age: "24", isActive: 1 },
    //     { id: 2, name: "Kavin", age: "24", isActive: 0 },
    //     { id: 3, name: "Sandun", age: "18", isActive: 1 },
    //     { id: 4, name: "Shehan", age: "28", isActive: 0 },
    // ];

    const [data, setData] = useState([]); // Initialize state with useState

    useEffect(() => {
        getdata();
    }, []);

    const getdata = () => {
        axios.get('https://localhost:7181/api/Employee')
        .then((result)=>{
            setData(result.data)
        })
        .catch((error)=>{
            console.log(error);
        })
    }

    const handleEdit = (id) => {
        handleShowEdit();
        axios.get(`https://localhost:7181/api/Employee/${id}`)
        .then((result)=>{
            setEditName(result.data.name);
            setEditAge(result.data.age);
            setEditIsActive(result.data.isActive);
            setEditId(id);

        })
        .catch((error)=>{
            console.log(error);
        })

    }

    const handleDelete = (id) => {
        if (window.confirm("Are you sure to delete this employee") === true) {
            axios.delete(`https://localhost:7181/api/Employee/${id}`)
            .then((result)=>{
                if(result.status === 200){
                    getdata();
                    toast.success('Employee has been deleted');
                }
            })
            .catch((error)=> {
                toast.error(error);
            })
        }

    }

    const handleUpdate = () => {
        const url = `https://localhost:7181/api/Employee/${editId}`;
        const data = {
            "id" : editId,
            "name" : editName,
            "age" : editAge,
            "isActive" : editIsActive
        }
        axios.put(url,data)
        .then((result)=>{
            handleCloseEdit();
            getdata();
            clear();
            toast.success('Employee has been updated');

        })
        .catch((error)=> {
            toast.error(error);
        })
        
    }

    
    const handleCreate = () => {
        // alert(id);
        handleShowCreate();
    }

    const handleSave = () => {
        const url = 'https://localhost:7181/api/Employee';
        const data = {
            "name" : name,
            "age" : age,
            "isActive" : isActive
        }
        axios.post(url,data)
        .then((result)=>{
            handleCloseCreate();
            getdata();
            clear();
            toast.success('Employee has been added');

        })
        .catch((error)=> {
            toast.error(error);
        })
    }

    const clear = () => {
        setName('');
        setAge('');
        setIsActive(0);
        setEditAge('');
        setEditName('');
        setEditIsActive(0);
    }

    const handleEditActiveChange = (e) => {
        setEditIsActive(e.target.checked ? 1 : 0);
    };

    const handleCreateActiveChange = (e) => {
        setIsActive(e.target.checked ? 1 : 0);
    };

    


    return (
        <Fragment>
            <ToastContainer/>
            <button onClick={handleCreate} className="btn btn-primary">Add Employee</button>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Name</th>
                        <th>Age</th>
                        <th>IsActive</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {data && data.length > 0 ? (
                        data.map((item, index) => (
                            <tr key={index}>
                                <td>{index + 1}</td>
                                <td>{item.name}</td>
                                <td>{item.age}</td>
                                <td>{item.isActive}</td>
                                <td colSpan={2}>
                                    <button onClick={() => handleEdit(item.id)} className="btn btn-primary">Edit</button>
                                    &nbsp; <button onClick={() => handleDelete(item.id)} className="btn btn-danger">Delete</button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="5">Loading...</td>
                        </tr>
                    )}
                </tbody>
            </Table>
            {/* Edit Modal */}
            <Modal show={showEdit} onHide={handleCloseEdit}>
                <Modal.Header closeButton>
                    <Modal.Title>Edit Employee</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="formBasicEmail">
                            <Form.Label>Name</Form.Label>
                            
                            <Form.Control type="text" placeholder="Enter name" value={editName} onChange={(e) => setEditName(e.target.value)}/>
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="formBasicPassword">
                            <Form.Label>Age</Form.Label>
                            <Form.Control type="text" placeholder="Enter age" value={editAge} onChange={(e) => setEditAge(e.target.value)}/>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formBasicCheckbox">
                            <Form.Check type="checkbox" label="IsActive" checked={editIsActive === 1 ? true : false} onChange={(e) => handleEditActiveChange(e)} value={editIsActive}/>
                        </Form.Group>
                        {/* <Button variant="primary" type="submit">
                            Update
                        </Button> */}
                    </Form>
                </Modal.Body>

                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseEdit}>Close</Button>
                    <Button variant="primary" onClick={handleUpdate}>Update</Button>
                </Modal.Footer>
            </Modal>

            {/* Create Modal */}
            <Modal show={showCreate} onHide={handleCloseCreate}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Employee</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <Form>
                    <Form.Group className="mb-3" controlId="formBasicEmail">
                            <Form.Label>Name</Form.Label>
                            
                            <Form.Control type="text" placeholder="Enter name" value={name} onChange={(e) => setName(e.target.value)}/>
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="formBasicPassword">
                            <Form.Label>Age</Form.Label>
                            <Form.Control type="text" placeholder="Enter age" value={age} onChange={(e) => setAge(e.target.value)}/>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formBasicCheckbox">
                            <Form.Check type="checkbox" label="IsActive" checked={isActive === 1 ? true : false} onChange={(e) => handleCreateActiveChange(e)} value={isActive}/>
                        </Form.Group>
                        {/* <Button variant="primary" type="submit">
                            Update
                        </Button> */}
                    </Form>
                </Modal.Body>

                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseCreate}>Close</Button>
                    <Button variant="primary" onClick={()=> handleSave()}>Submit</Button>
                </Modal.Footer>
            </Modal>
        </Fragment>
    );
};

export default CRUD;
