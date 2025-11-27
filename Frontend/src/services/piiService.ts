const API_URL = import.meta.env.VITE_API_URL;

export async function getEmailCount(): Promise<number> {
  const res = await fetch(`${API_URL}/getEmailCount`);
  return await res.json();
}

export async function sendSubmission(text: string) {
    const res = await fetch(`${API_URL}/sendSubmission`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ text }),
    })
  }