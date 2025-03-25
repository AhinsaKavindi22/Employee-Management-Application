import React, { Fragment, useState, useEffect } from "react";
import Table from 'react-bootstrap/Table';
import "bootstrap/dist/css/bootstrap.min.css";
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios from 'axios';
import { ToastContainer, toast } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css'


const Department = () => {
    const [showEdit, setShowEdit] = useState(false);
    const [showCreate, setShowCreate] = useState(false);

    const handleCloseEdit = () => setShowEdit(false);
    const handleCloseCreate = () => setShowCreate(false);
    const handleShowEdit = () => setShowEdit(true);
    const handleShowCreate = () => setShowCreate(true);

    const [name, setName] = useState('')
   

    const [editId, setEditId] = useState('')
    const [editName, setEditName] = useState('')
    

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
        axios.get('https://localhost:7181/api/Department')
            .then((result) => {
                setData(result.data)
            })
            .catch((error) => {
                console.log(error);
            })
    }

    const handleEdit = (id) => {
        handleShowEdit();
        axios.get(`https://localhost:7181/api/Department/${id}`)
            .then((result) => {
                setEditName(result.data.name);
                setEditId(id);

            })
            .catch((error) => {
                console.log(error);
            })

    }

    const handleDelete = (id) => {
        if (window.confirm("Are you sure to delete this Department") === true) {
            axios.delete(`https://localhost:7181/api/Department/${id}`)
                .then((result) => {
                    if (result.status === 200) {
                        getdata();
                        toast.success('Department has been deleted');
                    }
                })
                .catch((error) => {
                    toast.error(error);
                })
        }

    }

    const handleUpdate = () => {
        const url = `https://localhost:7181/api/Department/${editId}`;
        const data = {
            "id": editId,
            "name": editName,
            
        }
        axios.put(url, data)
            .then((result) => {
                handleCloseEdit();
                getdata();
                clear();
                toast.success('Department has been updated');

            })
            .catch((error) => {
                toast.error(error);
            })

    }


    const handleCreate = () => {
        // alert(id);
        handleShowCreate();
    }

    const handleSave = () => {
        const url = 'https://localhost:7181/api/Department';
        const data = {
            "name": name,
            
        }
        axios.post(url, data)
            .then((result) => {
                handleCloseCreate();
                getdata();
                clear();
                toast.success('Department has been added');

            })
            .catch((error) => {
                toast.error(error);
            })
    }

    const clear = () => {
        setName('');
        setEditName('');
    }


    return (
        <div className="p-3 mt-3">
            <Fragment>
                <ToastContainer />
                {/* Button positioned at the top right corner */}
                <div className="d-flex justify-content-end mb-3">
                    <button onClick={handleCreate} className="btn btn-primary">Add Department</button>
                </div>
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Name</th>

                        </tr>
                    </thead>
                    <tbody>
                        {data && data.length > 0 ? (
                            data.map((item, index) => (
                                <tr key={index}>
                                    <td>{index + 1}</td>
                                    <td>{item.name}</td>

                                    <td>
                                        <button onClick={() => handleEdit(item.id)} className="btn btn-primary">Edit</button>
                                        &nbsp; &nbsp;
                                        <button onClick={() => handleDelete(item.id)} className="btn btn-danger">Delete</button>
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan="6">Loading...</td>
                            </tr>
                        )}
                    </tbody>
                </Table>
                {/* Edit Modal */}
                <Modal show={showEdit} onHide={handleCloseEdit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Edit Department</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form>
                            <Form.Group className="mb-3" controlId="formBasicEmail">
                                <Form.Label>Name</Form.Label>
                                <Form.Control type="text" placeholder="Enter name" value={editName} onChange={(e) => setEditName(e.target.value)} />
                            </Form.Group>
                            
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
                        <Modal.Title>Add Department</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form>
                            <Form.Group className="mb-3" controlId="formBasicEmail">
                                <Form.Label>Name</Form.Label>
                                <Form.Control type="text" placeholder="Enter name" value={name} onChange={(e) => setName(e.target.value)} />
                            </Form.Group>
                           
                        </Form>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleCloseCreate}>Close</Button>
                        <Button variant="primary" onClick={handleSave}>Submit</Button>
                    </Modal.Footer>
                </Modal>
            </Fragment>
        </div>

    );
};

export default Department;
