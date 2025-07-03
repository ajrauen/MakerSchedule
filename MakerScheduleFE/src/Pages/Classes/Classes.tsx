import { ClassCard } from "@ms/Pages/Classes/ClassCard/ClassCard";
import { type EventOffering } from "@ms/types/event.types";

const Classes = () => {
  // Dummy EventOffering data
  const dummyEvents: EventOffering[] = [
    {
      eventName: "Yoga Basics",
      description:
        "Introduction to yoga for beginners. Learn basic poses and breathing techniques. This class will guide you through foundational yoga postures, focusing on alignment, flexibility, and breath control. You will also learn about the history and philosophy of yoga, how to set up a home practice, and tips for injury prevention. The session includes a gentle warm-up, a series of beginner-friendly asanas, and a relaxing cool-down. No prior experience is necessary, and all equipment is provided. Join us to start your journey toward improved balance, strength, and mindfulness.",
      leaders: [1, 2],
      attendees: [5, 6, 7, 8],
      scheduleStart: Date.now() + 24 * 60 * 60 * 1000, // Tomorrow
      duration: 60 * 60 * 1000, // 1 hour
      price: 15,
    },
    {
      eventName: "Advanced Pottery",
      description:
        "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.",
      leaders: [3],
      attendees: [9, 10, 11],
      scheduleStart: Date.now() + 2 * 24 * 60 * 60 * 1000, // Day after tomorrow
      duration: 120 * 60 * 1000, // 2 hours
      price: 40,
    },
    {
      eventName: "Woodworking Workshop",
      description:
        "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.",
      leaders: [4, 5],
      attendees: [12, 13, 14, 15, 16],
      scheduleStart: Date.now() + 3 * 24 * 60 * 60 * 1000, // 3 days from now
      duration: 180 * 60 * 1000, // 3 hours
      price: 55,
    },
    {
      eventName: "Digital Art Fundamentals",
      description:
        "Introduction to digital art using tablets and software. No experience required. This class will cover the basics of digital drawing, painting, and illustration using popular software tools. You will learn about layers, brushes, color theory, and composition, as well as how to use shortcuts and customize your workspace for efficiency. The instructor will provide step-by-step demonstrations and personalized feedback on your work. Bring your own tablet or use one of ours. By the end of the session, you will have completed several digital sketches and gained confidence in your creative abilities. Perfect for aspiring digital artists and hobbyists alike.",
      leaders: [6],
      attendees: [17, 18, 19, 20],
      scheduleStart: Date.now() + 5 * 24 * 60 * 60 * 1000, // 5 days from now
      duration: 90 * 60 * 1000, // 1.5 hours
      price: 25,
    },
    {
      eventName: "Cooking Masterclass",
      description:
        "Learn to cook authentic Italian pasta dishes from scratch. This masterclass will take you through the process of making fresh pasta dough, rolling and shaping different types of pasta, and preparing classic sauces such as marinara, Alfredo, and pesto. The chef will share tips on ingredient selection, timing, and plating for restaurant-quality results. Participants will work in small groups and enjoy a tasting session at the end of the class. All ingredients and equipment are provided. Whether you are a beginner or a seasoned cook, you will leave with new skills and delicious recipes to try at home.",
      leaders: [7, 8],
      attendees: [21, 22, 23, 24, 25, 26],
      scheduleStart: Date.now() + 7 * 24 * 60 * 60 * 1000, // 1 week from now
      duration: 150 * 60 * 1000, // 2.5 hours
      price: 35,
    },
  ];

  return (
    <div className="flex flex-row gap-4 bg-[#F2F4EF] justify-center w-full min-h-screen">
      <div className="w-1/6">filter</div>
      <div className="flex flex-col gap-4  lg:max-w-[880px] w-full mx-auto">
        {/* <ClassCard event={dummyEvents[0]} /> */}
        {dummyEvents.map((event, index) => (
          <ClassCard key={index} event={event} />
        ))}
      </div>
    </div>
  );
};

export { Classes };
