import { apiRoot, apiRoutes } from "../../definitions";
import { useToDoAppi } from "../../hooks/useToDoAppi";
import { ToDoTask } from "../../interfaces";
import { Verbs } from "../../utils";

interface ToDoCardProps {
    toDoTask: ToDoTask;
    onClick: (task:ToDoTask) => void;
    reload: () => void;
}

export const ToDoCard = ({ toDoTask, onClick, reload }: ToDoCardProps) => {
    const [deleteTask] = useToDoAppi(Verbs.Delete, apiRoot, apiRoutes.toDo);

    const onDelete = async () => {
        try {
            await deleteTask({ param: toDoTask.id!.toString()});
            reload();
        }
        catch (e) {
            console.log(e);
        }
    }

    return (
        <div key={`todo-card-${toDoTask.id}`}>
            <h2>{toDoTask.description}</h2>
            <p>Due Date: {toDoTask.dueDate ? new Date(toDoTask.dueDate).toLocaleDateString() : ''}</p>
            <button onClick={onDelete}>Delete</button>
            <button onClick={() => onClick(toDoTask)}>Edit</button>
        </div>
    )
}