import { useEffect, useState } from "react";
import { ToDoTask } from "../../interfaces";
import { useToDoAppi } from "../../hooks/useToDoAppi";
import { apiRoot, apiRoutes } from "../../definitions";
import { groupBy, Verbs } from "../../utils";
import { ToDoCard } from "../ToDoCard";
import { UpdateTaskForm } from "../UpdateTaskForm";
import { CreateTaskForm } from "../CreateTaskForm";

export const ToDoList = () => {
    const [todos, setTodos] = useState<ToDoTask[]>([]);
    const [fetchApi, isLoading] = useToDoAppi(Verbs.Get, apiRoot, apiRoutes.toDo);
    const [error, setError] = useState<string>('');
    const [selecedTask, setSelectedTask] = useState<ToDoTask | null>(null);
    const [isCreating, setIsCreating] = useState(false);
    const onCardClick = (task: ToDoTask) => setSelectedTask(task);

    const getList = async () => {
        try {
            const response = await fetchApi();
            setTodos(response);
        }
        catch (e) {
            setError('Failed to get ToDo list');
            console.log(e);
        }
    }

    useEffect(() => {
        getList();
    }, []);

    const onDismiss = (shouldRefresh: boolean) => {
        setSelectedTask(null);
        setIsCreating(false);
        if (shouldRefresh) getList();
    }

    const toDoByStatus = groupBy(todos, 'statusId');
    return (
        <>
            {isLoading
                ? (<p>Loading...</p>)
                : (
                    <>
                        <h1>To do taks <span><button onClick={() => setIsCreating(true)}>Create</button></span></h1>
                        {error && <p>{error}</p>}
                        <div style={{ display: 'flex', justifyContent: 'space-around' }}>
                            <div>
                                <h2>Not Started</h2>
                                {toDoByStatus[1]?.map((todo) => (
                                    <ToDoCard key= {`todo-card-${todo.id}`} toDoTask={todo} onClick={onCardClick} reload={getList} />
                                ))}
                            </div>
                            <div>
                                <h2>In Progress</h2>
                                {toDoByStatus[2]?.map((todo) => (
                                    <ToDoCard key= {`todo-card-${todo.id}`} toDoTask={todo} onClick={onCardClick} reload={getList}/>
                                ))}
                            </div>
                            <div>
                                <h2>Done</h2>
                                {toDoByStatus[3]?.map((todo) => (
                                    <ToDoCard key= {`todo-card-${todo.id}`} toDoTask={todo} onClick={onCardClick} reload={getList}/>
                                ))}
                            </div>
                            <div>
                                <h2>Overdue</h2>
                                {toDoByStatus[4]?.map((todo) => (
                                    <ToDoCard key= {`todo-card-${todo.id}`} toDoTask={todo} onClick={onCardClick} reload={getList}/>
                                ))}
                            </div>
                        </div>
                        {selecedTask && <UpdateTaskForm todoTask={selecedTask} onDismiss={onDismiss} />}
                        {isCreating && <CreateTaskForm onDismiss={onDismiss} />}
                    </>
                )}
        </>
    )
}