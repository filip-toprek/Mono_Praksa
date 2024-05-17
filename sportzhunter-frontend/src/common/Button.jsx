export default function Button({ children, type="submit", onClick=null, style=null}) {
    return <button style={style} className="Button" type={type} onClick={onClick}>{children}</button>;
}